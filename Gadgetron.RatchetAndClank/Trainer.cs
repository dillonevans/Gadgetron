using Gadgetron.Ps2;
using Gadgetron.RatchetAndClank.Inventory;
using Gadgetron.RatchetAndClank.Planets;
using Microsoft.Extensions.Logging;

namespace Gadgetron.RatchetAndClank
{
    public class Trainer
    {
        #region Constants

        private const byte DisabledFlag = 0x00;
        private const byte EnabledFlag = 0x01;
        private const int BoltCountAddress = 0x2015ED98;
        private const int CurrentPlanetAddress = 0x2015ED84;

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

            await this.Unlock(Item.Items);
            await this.Unlock(Weapon.Weapons);
            await this.Unlock(Gadget.Gadgets);

            Planet? currentPlanet = await this.GetCurrentPlanet();

            if (currentPlanet is not null)
            {
                this.logger.LogInformation("The current planet is {Planet}.", currentPlanet.Name);
            }
        }

        public async Task Lock(IEnumerable<IInventoryItem> items)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (IInventoryItem item in items)
            {
                await this.Lock(item);
            }
        }

        public async Task Unlock(IEnumerable<IInventoryItem> items)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (IInventoryItem item in items)
            {
                await this.Unlock(item);
            }
        }

        public async Task<Planet?> GetCurrentPlanet()
        {
            int id = await this.client.ReadInt32Async(CurrentPlanetAddress);
            return Planet.GetById(id);
        }

        public async Task Lock(IInventoryItem inventoryItem)
        {
            ArgumentNullException.ThrowIfNull(inventoryItem, nameof(inventoryItem));

            this.logger.LogInformation("Removing the {Item}.", inventoryItem.Name);
            await this.client.WriteInt8Async(inventoryItem.CheckAddress, DisabledFlag);
        }

        public async Task Unlock(IInventoryItem inventoryItem)
        {
            ArgumentNullException.ThrowIfNull(inventoryItem, nameof(inventoryItem));

            this.logger.LogInformation("Giving Ratchet the {Item}.", inventoryItem.Name);
            await this.client.WriteInt8Async(inventoryItem.CheckAddress, EnabledFlag);
        }

        public async Task SetAmmo(Weapon weapon, int amount)
        {
            ArgumentNullException.ThrowIfNull(weapon, nameof(weapon));

            if (!weapon.AmmoAddress.HasValue)
            {
                throw new ArgumentException("Weapon does not use ammo.", nameof(weapon));
            }

            this.logger.LogInformation("Setting {Weapon} ammo count to {Count}", weapon.Name, amount);
            await this.client.WriteInt32Async(weapon.AmmoAddress.Value, amount);
        }

        public async Task MaxOutWeapons()
        {
            IEnumerable<Weapon> weaponsWithAmmo = Weapon.Weapons.Where(weapon => weapon.MaximumAmmo.HasValue);

            foreach (Weapon weapon in weaponsWithAmmo)
            {
                await this.SetAmmo(weapon, weapon.MaximumAmmo!.Value);
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
