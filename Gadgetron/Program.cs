using Gadgetron.Ps2;

namespace Gadgetron
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ModifyBoltsExample();
            Console.ReadLine();
        }

        public static async Task ModifyBoltsExample()
        {
            int boltCountAddress = 0x2015ED98;
            int bombGloveAmmoAddress = 0x2013D450;
            int rynoAddress = 0x2013D4D7;
            int visibombGunAddress = 0x2013D4CD;
            await using var client = new Pcsx2Client();

            await client.ConnectAsync();

            int currentBoltCount = await client.ReadInt32Async(boltCountAddress);
            Console.WriteLine($"Current bolt count: {currentBoltCount}");

            int additionalBolts = 100;
            Console.WriteLine($"Incrementing bolt count to {currentBoltCount + additionalBolts}");

            await client.WriteInt32Async(boltCountAddress, currentBoltCount + additionalBolts);
            await client.WriteInt32Async(bombGloveAmmoAddress, 50);
            await client.WriteInt8Async(rynoAddress, 1);
            int hasRyno = await client.ReadInt8Async(rynoAddress);
            Console.WriteLine(hasRyno);
            
            await client.WriteInt8Async(visibombGunAddress, 1);
            int hasVisibomb = await client.ReadInt8Async(visibombGunAddress);
            Console.WriteLine(hasVisibomb);
        }
    }
}
