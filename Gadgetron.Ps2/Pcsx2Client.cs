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
        private const int Port = 28011;
        private readonly TcpClient client;
        private readonly Lazy<NetworkStream> networkStream;

        public Pcsx2Client()
        {
            this.client = new TcpClient(AddressFamily.InterNetwork);
            this.networkStream = new Lazy<NetworkStream>(() => this.client.GetStream());
        }

        public async Task Connect(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Connecting to PCSX2 emulator...");
            await this.client.ConnectAsync(IPAddress.Loopback, Port, cancellationToken);
            Console.WriteLine("Successfully connected to PCSX2 emulator.");
        }

        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("Disposing");

            if (this.networkStream.IsValueCreated)
            {
                await this.networkStream.Value.DisposeAsync().ConfigureAwait(false);
            }

            this.client.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Reads a 32 bit integer from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to read from.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns>The value stored at the <paramref name="address"/>.</returns>
        public async Task<int> ReadInt32(int address, CancellationToken cancellationToken = default)
        {
            byte[] bytes =
            [
                .. BitConverter.GetBytes(9),
                (byte)PineCommand.Read32Bits,
                .. BitConverter.GetBytes(address)
            ];

            await this.networkStream.Value.WriteAsync(bytes, cancellationToken);

            byte[] buffer = new byte[9];

            await this.networkStream.Value.ReadExactlyAsync(buffer, cancellationToken);
            return BitConverter.ToInt32(buffer.AsSpan()[^4..]);
        }

        /// <summary>
        /// Reads a 32 bit integer from the <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The address in RAM to write the <paramref name="value"/>.</param>
        /// <param name="cancellationToken">The optional cancellation token.</param>
        /// <returns><see langword="true"/> if the <paramref name="value"/> was written successfully.</returns>
        public async Task<bool> WriteInt32(int address, int value, CancellationToken cancellationToken = default)
        {
            byte[] bytes =
            [
                .. BitConverter.GetBytes(13), 
                (byte)PineCommand.Write32Bits,
                .. BitConverter.GetBytes(address),
                .. BitConverter.GetBytes(value)
            ];
            
            await this.networkStream.Value.WriteAsync(bytes, cancellationToken);

            byte[] buffer = new byte[5];
            await this.networkStream.Value.ReadExactlyAsync(buffer, cancellationToken);

            return buffer[4] == 0;
        }
    }
}
