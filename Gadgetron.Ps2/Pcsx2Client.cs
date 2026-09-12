using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace Gadgetron.Ps2
{
    /// <summary>
    /// The TCP client for interfacing with the PCSX2 emulator.
    /// </summary>
    /// <remarks>
    /// This client utilizes the PINE protocol in order to communicate
    /// with the PCSX2 server. The official draft for this protocol can be found
    /// here <see href="https://github.com/GovanifY/pine/blob/master/standard/draft.dtd"/>.
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
        /// Reads a byte from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to read from.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/> containing the <see cref="byte"/>.</returns>
        public async Task<byte> ReadInt8Async(int address, CancellationToken cancellationToken = default)
        {
            byte[] bytes =
            [
                .. BitConverter.GetBytes(9),
                (byte)PineCommand.Read8Bits,
                .. BitConverter.GetBytes(address)
            ];

            await this.networkStream.Value.WriteAsync(bytes, cancellationToken);
            
            byte[] response = await this.ReadResponseAsync(cancellationToken);
            return response[^1];
        }

        /// <summary>
        /// Writes the <paramref name="value"/> to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to write the <paramref name="value"/> to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task WriteInt8Async(int address, byte value, CancellationToken cancellationToken = default)
        {
            byte[] bytes =
            [
                .. BitConverter.GetBytes(10),
                (byte)PineCommand.Write8Bits,
                .. BitConverter.GetBytes(address),
                value
            ];

            await this.networkStream.Value.WriteAsync(bytes, cancellationToken);
            await this.ReadResponseAsync(cancellationToken);
        }

        /// <summary>
        /// Reads a 32 bit integer from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to read from.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/> containing the <see cref="int"/>.</returns>
        public async Task<int> ReadInt32Async(int address, CancellationToken cancellationToken = default)
        {
            byte[] bytes =
            [
                .. BitConverter.GetBytes(9),
                (byte)PineCommand.Read32Bits,
                .. BitConverter.GetBytes(address)
            ];

            await this.networkStream.Value.WriteAsync(bytes, cancellationToken);
            byte[] response = await this.ReadResponseAsync(cancellationToken);
            return BitConverter.ToInt32(response.AsSpan()[^4..]);
        }

        /// <summary>
        /// Reads a 32 bit integer from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to write the <paramref name="value"/> to.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task WriteInt32Async(int address, int value, CancellationToken cancellationToken = default)
        {
            byte[] bytes =
            [
                .. BitConverter.GetBytes(13),
                (byte)PineCommand.Write32Bits,
                .. BitConverter.GetBytes(address),
                .. BitConverter.GetBytes(value)
            ];

            await this.networkStream.Value.WriteAsync(bytes, cancellationToken);
            await this.ReadResponseAsync(cancellationToken);
        }

        /// <summary>
        /// Reads a response from the network stream and returns it.
        /// </summary>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>The entire response.</returns>
        private async Task<byte[]> ReadResponseAsync(CancellationToken cancellationToken = default)
        {
            // The first 4 bytes contain the total message size including
            // the bytes allocated for this array.
            byte[] messageSizeBuffer = new byte[sizeof(int)];
            await this.networkStream.Value.ReadExactlyAsync(messageSizeBuffer, cancellationToken);

            // The next byte contains the result code which indicates success or failure.
            byte[] resultBuffer = new byte[sizeof(byte)];
            await this.networkStream.Value.ReadExactlyAsync(resultBuffer, cancellationToken);

            // The remainder of the response contains the "argument" which is the actual result.
            int totalMessageSize = BitConverter.ToInt32(messageSizeBuffer);
            int argumentSize = totalMessageSize - resultBuffer.Length - messageSizeBuffer.Length;
            byte[] argumentBuffer = new byte[argumentSize];
            await this.networkStream.Value.ReadExactlyAsync(argumentBuffer, cancellationToken);

            // Return the full response.
            return [.. messageSizeBuffer, .. resultBuffer, ..argumentBuffer];
        }

        #endregion
    }
}
