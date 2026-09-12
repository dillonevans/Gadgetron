using Gadgetron.Ps2;
using Microsoft.Extensions.Logging;

namespace Gadgetron.RatchetAndClank
{
    public class Trainer
    {
        #region Fields

        private readonly ILogger<Trainer> logger;
        private readonly Pcsx2Client client;

        #endregion

        #region Constructor

        public Trainer(ILogger<Trainer> logger, Pcsx2Client client)
        {
            this.logger = logger;
            this.client = client;
        }

        #endregion

        #region Methods

        public async Task RunAsync()
        {
            int boltCountAddress = 0x2015ED98;
            int rynoAddress = 0x2013D4D7;

            await this.client.ConnectAsync();

            this.logger.LogDebug("Reading bolt count from {Address:X}", boltCountAddress);
            int currentBoltCount = await this.client.ReadInt32Async(boltCountAddress);
            this.logger.LogInformation("Current bolt count: {BoltCount}", currentBoltCount);

            int targetBoltCount = currentBoltCount + 100;
            this.logger.LogInformation("Incrementing bolt count to {UpdatedCount}", targetBoltCount);
            this.LogAddressWrite(boltCountAddress, targetBoltCount);
            await this.client.WriteInt32Async(boltCountAddress, targetBoltCount);

            this.logger.LogInformation("Giving Ratchet the RYNO");
            this.LogAddressWrite(rynoAddress, 1);
            await this.client.WriteInt8Async(rynoAddress, 1);
        }

        private void LogAddressWrite<T>(int address, T value)
        {
            this.logger.LogDebug("Writing {Value:X} to {Address:X}", value, address);
        }

        #endregion
    }
}
