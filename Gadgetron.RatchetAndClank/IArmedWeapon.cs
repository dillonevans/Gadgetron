namespace Gadgetron.RatchetAndClank
{
    /// <summary>
    /// Represents a weapon in the game that requires ammo.
    /// </summary>
    public interface IArmedWeapon : IWeapon
    {
        /// <summary>
        /// The address in RAM in which the ammo count is stored.
        /// </summary>
        int AmmoAddress { get; }

        /// <summary>
        /// The maximum ammo capacity for the weapon.
        /// </summary>
        int MaximumAmmo { get; }
    }
}
