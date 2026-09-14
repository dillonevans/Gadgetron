using Microsoft.Extensions.Logging;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;

namespace Gadgetron.Ps2
{
    /// <summary>
    /// The TCP client for interfacing with the PCSX2 emulator.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This client utilizes the PINE protocol in order to communicate
    ///         with the PCSX2 server. The official draft for this protocol can be found
    ///         here <see href="https://github.com/GovanifY/pine/blob/master/standard/draft.dtd"/>.
    ///     </para>
    ///     <para>
    ///         Because the PlayStation 2 uses little-endian byte order, this client ensures
    ///         that all byte operations use little-endian ordering regardless of the
    ///         current platform's endianness.
    ///     </para>
    /// </remarks>
    public class Pcsx2Client : IAsyncDisposable
    {
        #region Constants

        private const int Port = 28011;

        #endregion

        #region Methods

        private readonly TcpClient client;
        private readonly Lazy<NetworkStream> networkStream;
        private readonly ILogger<Pcsx2Client> logger;

        #endregion

        #region Constructor

        public Pcsx2Client(ILogger<Pcsx2Client> logger)
        {
            this.client = new TcpClient(AddressFamily.InterNetwork);
            this.networkStream = new Lazy<NetworkStream>(() => this.client.GetStream());
            this.logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Connects to the PCSX2 emulator's server.
        /// </summary>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Connecting to PCSX2 emulator on port {Port}...", Port);
            await this.client.ConnectAsync(IPAddress.Loopback, Port, cancellationToken);
            this.logger.LogInformation("Successfully connected to PCSX2 emulator on port {Port}.", Port);
        }

        /// <summary>
        /// Disposes the resources used by the client.
        /// </summary>
        /// <returns>An awaitable <see cref="ValueTask"/>.</returns>
        public async ValueTask DisposeAsync()
        {
            if (this.networkStream.IsValueCreated)
            {
                await this.networkStream.Value.DisposeAsync().ConfigureAwait(false);
            }

            this.client.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes the <paramref name="value"/> to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to write the <paramref name="value"/> to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task WriteInt8Async(uint address, byte value, CancellationToken cancellationToken = default)
        {
            byte[] arguments = [.. GetBytes(address), value];
            await this.SendMessageAsync(PineCommand.Write8Bits, arguments, cancellationToken);
        }

        /// <summary>
        /// Reads a 32 bit integer from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to write the <paramref name="value"/> to.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task WriteInt32Async(uint address, int value, CancellationToken cancellationToken = default)
        {
            byte[] arguments = [.. GetBytes(address), .. GetBytes(value)];
            await this.SendMessageAsync(PineCommand.Write32Bits, arguments, cancellationToken);
        }

        /// <summary>
        /// Reads a byte from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to read from.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/> containing the <see cref="byte"/>.</returns>
        public async Task<byte> ReadInt8Async(uint address, CancellationToken cancellationToken = default)
        {
            // Read the response and return the last byte.
            byte[] response = await this.SendMessageAsync(PineCommand.Read8Bits, GetBytes(address), cancellationToken);
            return response[^sizeof(byte)];
        }

        /// <summary>
        /// Reads a 32 bit integer from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to read from.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/> containing the <see cref="int"/>.</returns>
        public async Task<int> ReadInt32Async(uint address, CancellationToken cancellationToken = default)
        {
            byte[] response = await this.SendMessageAsync(PineCommand.Read32Bits, GetBytes(address), cancellationToken);
            return GetInt32(response[^sizeof(int)..]);
        }

        /// <summary>
        /// Creates a message using the <paramref name="command"/> and <paramref name="arguments"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The message.</returns>
        private static byte[] CreateMessage(PineCommand command, byte[] arguments)
        {
            int length = sizeof(int) + sizeof(byte) + arguments.Length;
            return [.. GetBytes(length), (byte)command, .. arguments];
        }

        /// <summary>
        /// Sends a message to the server built from the specified <paramref name="command"/> and <paramref name="arguments"/>.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <param name="arguments">The arguments for the command.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>The response from the server.</returns>
        private async Task<byte[]> SendMessageAsync(PineCommand command, byte[] arguments, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));

            // Create the message and send it to the server.
            byte[] message = CreateMessage(command, arguments);

            this.logger.LogDebug("Sending message {Message}.", message);
            await this.networkStream.Value.WriteAsync(message, cancellationToken);
            
            // Each sent message is guaranteed to have a response.
            return await this.ReadMessageAsync(cancellationToken);
        }

        /// <summary>
        /// Reads a response message from the network stream and returns it.
        /// </summary>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>The entire response.</returns>
        private async Task<byte[]> ReadMessageAsync(CancellationToken cancellationToken = default)
        {
            // The first 4 bytes contain the total message size including
            // the bytes allocated for this array.
            byte[] messageSizeBuffer = new byte[sizeof(int)];
            await this.networkStream.Value.ReadExactlyAsync(messageSizeBuffer, cancellationToken);

            // The next byte contains the result code which indicates success or failure.
            byte[] resultBuffer = new byte[sizeof(byte)];
            await this.networkStream.Value.ReadExactlyAsync(resultBuffer, cancellationToken);

            // The remainder of the response contains the "argument" which is the actual result.
            int totalMessageSize = GetInt32(messageSizeBuffer);
            int argumentSize = totalMessageSize - resultBuffer.Length - messageSizeBuffer.Length;
            
            byte[] argumentBuffer = new byte[argumentSize];
            await this.networkStream.Value.ReadExactlyAsync(argumentBuffer, cancellationToken);

            // Return the full response.
            return [.. messageSizeBuffer, .. resultBuffer, ..argumentBuffer];
        }

        /// <summary>
        /// Converts the <paramref name="value"/> to a little-endian <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The array.</returns>
        private static byte[] GetBytes(uint value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(int)];

            BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
            return bytes.ToArray();
        }

        /// <summary>
        /// Converts the <paramref name="value"/> to a little-endian <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The array.</returns>
        private static byte[] GetBytes(int value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(int)];

            BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
            return bytes.ToArray();
        }

        /// <summary>
        /// Converts the <paramref name="bytes"/> as little-endian to an <see cref="int"/>.
        /// </summary>
        /// <param name="bytes">The bytes to convert.</param>
        /// <returns>The <see cref="int"/> value.</returns>
        private static int GetInt32(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes);
            return BinaryPrimitives.ReadInt32LittleEndian(bytes);
        }

        #endregion
    }
}
