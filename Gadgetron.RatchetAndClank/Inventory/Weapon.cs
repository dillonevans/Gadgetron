namespace Gadgetron.RatchetAndClank.Inventory
{
    /// <summary>
    /// A weapon in the game.
    /// </summary>
    public class Weapon : IInventoryItem
    {
        #region Fields

        public static readonly Weapon BombGlove = new("Bomb Glove", checkAddress: 0x2013D4CA, ammoAddress: 0x2013D450, maximumAmmo: 40);
        public static readonly Weapon Blaster = new("Blaster", checkAddress: 0x2013D4CF, ammoAddress: 0x2013D464, maximumAmmo: 200);
        public static readonly Weapon DecoyGlove = new("Decoy Glove", checkAddress: 0x2013D4D9, ammoAddress: 0x2013D48C, maximumAmmo: 20);
        public static readonly Weapon Devastator = new("Devastator", checkAddress: 0x2013D4CB, ammoAddress: 0x2013D454, maximumAmmo: 20);
        public static readonly Weapon DroneDevice = new("Drone Device", checkAddress: 0x2013D4D8, ammoAddress: 0x2013D488, maximumAmmo: 10);
        public static readonly Weapon GloveOfDoom = new("Glove of Doom", checkAddress: 0x2013D4D4, ammoAddress: 0x2013D478, maximumAmmo: 10);
        public static readonly Weapon MineGlove = new("Mine Glove", checkAddress: 0x2013D4D1, ammoAddress: 0x2013D46C, maximumAmmo: 50);
        public static readonly Weapon Pyrociter = new("Pyrociter", checkAddress: 0x2013D4D0, ammoAddress: 0x2013D468, maximumAmmo: 240);
        public static readonly Weapon Ryno = new("RYNO", checkAddress: 0x2013D4D7, ammoAddress: 0x2013D484, maximumAmmo: 50);
        public static readonly Weapon TeslaClaw = new("Tesla Claw", checkAddress: 0x2013D4D3, ammoAddress: 0x2013D474, maximumAmmo: 240);
        public static readonly Weapon MorphoRay = new("Morph-o-Ray", checkAddress: 0x2013D4D5);
        public static readonly Weapon SuckCannon = new("Suck Cannon", checkAddress: 0x2013D4C9);
        public static readonly Weapon Taunter = new("Taunter", checkAddress: 0x2013D4CE);
        public static readonly Weapon Walloper = new("Walloper", checkAddress: 0x2013D4D2);
        public static readonly Weapon VisibombGun = new("Visibomb Gun", checkAddress: 0x2013D4CD, ammoAddress: 0x2013D45C, maximumAmmo: 20);

        #endregion

        #region Properties

        /// <summary>
        /// Enumerates all weapons.
        /// </summary>
        public static IEnumerable<Weapon> Weapons =>
        [
            Blaster,
            BombGlove,
            DecoyGlove,
            Devastator,
            DroneDevice,
            GloveOfDoom,
            MineGlove,
            Pyrociter,
            Ryno,
            TeslaClaw,
            VisibombGun,
            Taunter,
            SuckCannon,
            Walloper,
            MorphoRay
        ];

        public string Name { get; private set; }
        public uint CheckAddress { get; private set; }
        public uint? AmmoAddress { get; private set; }
        public int? MaximumAmmo { get; private set; }

        #endregion

        #region Constructor

        private Weapon(string name, uint checkAddress)
        {
            this.Name = name;
            this.CheckAddress = checkAddress;
        }

        private Weapon(string name, uint checkAddress, uint ammoAddress, int maximumAmmo) : this(name, checkAddress)
        {
            this.AmmoAddress = ammoAddress;
            this.MaximumAmmo = maximumAmmo;
        }

        #endregion
    }
}
