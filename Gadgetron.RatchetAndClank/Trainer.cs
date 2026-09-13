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

            int currentBoltCount = await this.GetBolts();
            int targetBoltCount = currentBoltCount + 100;

            await this.SetBolts(targetBoltCount);
            await this.LockAllWeapons();
            await this.UnlockAllWeapons();
            await this.MaxOutWeapons();
        }

        public async Task<int> GetBolts()
        {
            return await this.client.ReadInt32Async(BoltCountAddress);
        }
        
        public async Task SetBolts(int amount)
        {
            await this.client.WriteInt32Async(BoltCountAddress, amount);
        }

        public async Task UnlockAllWeapons()
        {
            this.logger.LogInformation("Unlocking all weapons.");

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
            await this.client.WriteInt8Async(weapon.CheckAddress, DisabledFlag);
        }

        public async Task UnlockWeapon(IWeapon weapon)
        {
            this.logger.LogInformation("Giving Ratchet the {Weapon}.", weapon.Name);
            await this.client.WriteInt8Async(weapon.CheckAddress, EnabledFlag);
        }

        public async Task SetAmmo(IArmedWeapon weapon, int amount)
        {
            this.logger.LogInformation("Setting {Weapon} ammo count to {Count}", weapon.Name, amount);
            await this.client.WriteInt32Async(weapon.AmmoAddress, amount);
        }

        #endregion
    }
}
