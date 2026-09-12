using Gadgetron.Ps2;
using System.ComponentModel;

namespace Gadgetron
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new Pcsx2Client();

            await client.Connect();

            int boltCountAddress = 0x2015ED98;

            while (true)
            {
                int currentBoltCount = await client.ReadInt32(boltCountAddress);
                Console.WriteLine($"Current bolt count: {currentBoltCount}");
                
                int additionalBolts = 100;
                Console.WriteLine($"Incrementing bolt count to {currentBoltCount + additionalBolts}");

                await client.WriteInt32(boltCountAddress, currentBoltCount + additionalBolts);
                await Task.Delay(1000);
            }
        }
    }
}
