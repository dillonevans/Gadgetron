using System.Net;
using System.Net.Sockets;

namespace Gadgetron.Ps2
{
    /// <summary>
    /// The TCP client for interfacing with the PCSX2 emulator.
    /// </summary>
    public class Pcsx2Client
    {
        private const int Port = 28011;
        private readonly TcpClient client;
        private NetworkStream? stream;

        public Pcsx2Client()
        {
            this.client = new TcpClient(AddressFamily.InterNetwork);
        }

        public async Task Connect()
        {
            Console.WriteLine("Connecting to PCSX2 emulator...");
            await this.client.ConnectAsync(IPAddress.Loopback, Port);
            this.stream = this.client.GetStream();
            Console.WriteLine("Successfully connected to PCSX2 emulator.");
        }

        public async Task<int> ReadInt32(int address)
        {
            byte[] bytes = [.. BitConverter.GetBytes(9), (byte)PineCommand.Read32Bits, .. BitConverter.GetBytes(address)];
            await this.stream!.WriteAsync(bytes);

            byte[] buffer = new byte[9];

            await this.stream.ReadExactlyAsync(buffer);
            return BitConverter.ToInt32(buffer.AsSpan()[^4..]);
        }

        public async Task<bool> WriteInt32(int address, int value)
        {
            byte[] bytes =
            [
                .. BitConverter.GetBytes(13), 
                (byte)PineCommand.Write32Bits,
                .. BitConverter.GetBytes(address),
                .. BitConverter.GetBytes(value)
            ];
            await this.stream!.WriteAsync(bytes);

            byte[] buffer = new byte[5];

            await this.stream.ReadExactlyAsync(buffer);

            return buffer[4] == 0;
        }
    }
}
