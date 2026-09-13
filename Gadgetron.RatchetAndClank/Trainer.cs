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
            await this.UnlockAllItems();
        }

        public async Task UnlockAllWeapons()
        {
            foreach (IWeapon weapon in Weapons.All)
            {
                await this.UnlockWeapon(weapon);
            }
        }

        public async Task UnlockAllGadgets()
        {
            foreach (IGadget gadget in Gadgets.All)
            {
                await this.UnlockGadget(gadget);
            }
        }

        public async Task UnlockAllItems()
        {
            await this.UnlockAllWeapons();
            await this.UnlockAllGadgets();
        }

        public async Task LockAllWeapons()
        {
            foreach (IWeapon weapon in Weapons.All)
            {
                await this.LockWeapon(weapon);
            }
        }

        public async Task LockAllGadgets()
        {
            foreach (IGadget gadget in Gadgets.All)
            {
                await this.LockGadget(gadget);
            }
        }

        public async Task LockAllItems()
        {
            await this.LockAllWeapons();
            await this.LockAllGadgets();
        }

        public async Task LockWeapon(IWeapon weapon)
        {
            await this.LockItem(weapon);
        }

        public async Task UnlockWeapon(IWeapon weapon)
        {
            await this.UnlockItem(weapon);
        }

        public async Task UnlockGadget(IGadget gadget)
        {
            await this.UnlockItem(gadget);
        }

        public async Task LockGadget(IGadget gadget)
        {
            await this.LockItem(gadget);
        }

        public async Task LockItem(IInventoryItem item)
        {
            this.logger.LogInformation("Removing the {Item}.", item.Name);
            await this.client.WriteInt8Async(item.CheckAddress, DisabledFlag);
        }

        public async Task UnlockItem(IInventoryItem item)
        {
            this.logger.LogInformation("Giving Ratchet the {Item}.", item.Name);
            await this.client.WriteInt8Async(item.CheckAddress, EnabledFlag);
        }

        public async Task SetAmmo(IArmedWeapon weapon, int amount)
        {
            this.logger.LogInformation("Setting {Weapon} ammo count to {Count}", weapon.Name, amount);
            await this.client.WriteInt32Async(weapon.AmmoAddress, amount);
        }

        public async Task MaxOutWeapons()
        {
            foreach (IArmedWeapon weapon in Weapons.ArmedWeapons)
            {
                await this.SetAmmo(weapon, weapon.MaximumAmmo);
            }
        }

        public async Task<int> GetBolts()
        {
            return await this.client.ReadInt32Async(BoltCountAddress);
        }

        public async Task SetBolts(int amount)
        {
            await this.client.WriteInt32Async(BoltCountAddress, amount);
        }

        #endregion
    }
}
