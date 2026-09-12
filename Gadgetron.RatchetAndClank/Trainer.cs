using Gadgetron.Ps2;
using Microsoft.Extensions.Logging;

namespace Gadgetron.RatchetAndClank
{
    public class Trainer
    {
        #region Constants

        private const byte DisabledFlag = 0x00;
        private const byte EnabledFlag = 0x01;
        private const int BoltCountAddress = 0x2015ED98;

        #endregion

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
            await this.client.ConnectAsync();

            int currentBoltCount = await this.client.ReadInt32Async(BoltCountAddress);
            int targetBoltCount = currentBoltCount * 10;

            this.logger.LogInformation("Incrementing bolt count to {UpdatedCount}.", targetBoltCount);
            this.LogAddressWrite(BoltCountAddress, targetBoltCount);
            await this.client.WriteInt32Async(BoltCountAddress, targetBoltCount);

            await this.LockAllWeapons();
            await this.UnlockAllWeapons();
            await this.MaxOutWeapons();
        }
        
        public async Task SetBolts(int amount)
        {
            await this.client.WriteInt32Async(BoltCountAddress, amount);
        }

        public async Task UnlockAllWeapons()
        {
            foreach (IWeapon weapon in Weapons.All)
            {
                await this.UnlockWeapon(weapon);
            }
        }

        public async Task LockAllWeapons()
        {
            foreach (IWeapon weapon in Weapons.All)
            {
                await this.LockWeapon(weapon);
            }
        }

        public async Task MaxOutWeapons()
        {
            foreach (IArmedWeapon weapon in Weapons.ArmedWeapons)
            {
                await this.SetAmmo(weapon, weapon.MaximumAmmo);
            }
        }

        public async Task LockWeapon(IWeapon weapon)
        {
            this.logger.LogInformation("Removing the {Weapon}.", weapon.Name);
            this.LogAddressWrite(weapon.CheckAddress, DisabledFlag);
            await this.client.WriteInt8Async(weapon.CheckAddress, DisabledFlag);
        }

        public async Task UnlockWeapon(IWeapon weapon)
        {
            this.logger.LogInformation("Giving Ratchet the {Weapon}.", weapon.Name);
            this.LogAddressWrite(weapon.CheckAddress, EnabledFlag);
            await this.client.WriteInt8Async(weapon.CheckAddress, EnabledFlag);
        }

        public async Task SetAmmo(IArmedWeapon weapon, int amount)
        {
            this.LogAddressWrite(weapon.CheckAddress, amount);
            await this.client.WriteInt32Async(weapon.AmmoAddress, amount);
        }

        private void LogAddressWrite<T>(int address, T value)
        {
            this.logger.LogDebug("Writing {Value:X} to {Address:X}", value, address);
        }

        #endregion
    }
}
