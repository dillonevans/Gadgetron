namespace Gadgetron.RatchetAndClank
{
    /// <summary>A weapon in the game.</summary>
    /// <param name="Name">The name of the weapon.</param>
    /// <param name="CheckAddress"> The address in RAM in which the item check is performed. </param>
    public record SimpleWeapon(string Name, int CheckAddress) : IWeapon;

    ///<summary>A weapon in the game that requires ammo.</summary>
    /// <param name="Name">The name of the weapon.</param>
    /// <param name="CheckAddress"> The address in RAM in which the item check is performed. </param>
    /// <param name="AmmoAddress"> The address in RAM in which the ammo count is stored. </param>
    /// <param name="MaximumAmmo">The maximum ammo capacity for the weapon.</param>
    public record ArmedWeapon(string Name, int CheckAddress, int AmmoAddress, int MaximumAmmo) : IArmedWeapon;

    /// <summary>
    /// Every weapon in the game.
    /// </summary>
    public static class Weapons
    {
        public static readonly IArmedWeapon BombGlove = new ArmedWeapon(WeaponNames.BombGlove, CheckAddress: 0x2013D4CA, AmmoAddress: 0x2013D450, MaximumAmmo: 40);
        public static readonly IArmedWeapon Blaster = new ArmedWeapon(WeaponNames.Blaster, CheckAddress: 0x2013D4CF, AmmoAddress: 0x2013D464, MaximumAmmo: 200);
        public static readonly IArmedWeapon DecoyGlove = new ArmedWeapon(WeaponNames.DecoyGlove, CheckAddress: 0x2013D4D9, AmmoAddress: 0x2013D48C, MaximumAmmo: 20);
        public static readonly IArmedWeapon Devastator = new ArmedWeapon(WeaponNames.Devastator, CheckAddress: 0x2013D4CB, AmmoAddress: 0x2013D454, MaximumAmmo: 20);
        public static readonly IArmedWeapon DroneDevice = new ArmedWeapon(WeaponNames.DroneDevice, CheckAddress: 0x2013D4D8, AmmoAddress: 0x2013D488, MaximumAmmo: 10);
        public static readonly IArmedWeapon GloveOfDoom = new ArmedWeapon(WeaponNames.GloveOfDoom, CheckAddress: 0x2013D4D4, AmmoAddress: 0x2013D478, MaximumAmmo: 10);
        public static readonly IArmedWeapon MineGlove = new ArmedWeapon(WeaponNames.MineGlove, CheckAddress: 0x2013D4D1, AmmoAddress: 0x2013D46C, MaximumAmmo: 50);
        public static readonly IArmedWeapon Pyrociter = new ArmedWeapon(WeaponNames.Pyrociter, CheckAddress: 0x2013D4D0, AmmoAddress: 0x2013D468, MaximumAmmo: 240);
        public static readonly IArmedWeapon Ryno = new ArmedWeapon(WeaponNames.Ryno, CheckAddress: 0x2013D4D7, AmmoAddress: 0x2013D484, MaximumAmmo: 50);
        public static readonly IArmedWeapon TeslaClaw = new ArmedWeapon(WeaponNames.TeslaClaw, CheckAddress: 0x2013D4D3, AmmoAddress: 0x2013D474, MaximumAmmo: 240);
        public static readonly IWeapon MorphoRay = new SimpleWeapon(WeaponNames.MorphoRay, CheckAddress: 0x2013D4D5);
        public static readonly IWeapon SuckCannon = new SimpleWeapon(WeaponNames.SuckCannon, CheckAddress: 0x2013D4C9);
        public static readonly IWeapon Taunter = new SimpleWeapon(WeaponNames.Taunter, CheckAddress: 0x2013D4CE);
        public static readonly IWeapon Walloper = new SimpleWeapon(WeaponNames.Walloper, CheckAddress: 0x2013D4D2);
        public static readonly IArmedWeapon VisibombGun = new ArmedWeapon(WeaponNames.VisibombGun, CheckAddress: 0x2013D4CD, AmmoAddress: 0x2013D45C, MaximumAmmo: 20);

        /// <summary>
        /// Enumerates all weapons.
        /// </summary>
        /// <returns>Every weapon.</returns>
        public static IEnumerable<IWeapon> All => SimpleWeapons.Concat(ArmedWeapons);

        /// <summary>
        /// Enumerates all weapons that don't require ammo.
        /// </summary>
        /// <returns>All <see cref="SimpleWeapon"/> instances.</returns>
        public static IEnumerable<IWeapon> SimpleWeapons => [Taunter, SuckCannon, Walloper, MorphoRay];

        /// <summary>
        /// Enumerates all weapons that require ammo.
        /// </summary>
        /// <returns>All <see cref="ArmedWeapon"/> instances.</returns>
        public static IEnumerable<IArmedWeapon> ArmedWeapons =>
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
            VisibombGun
        ];
    }
}
