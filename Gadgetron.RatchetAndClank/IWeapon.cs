using System;
using System.Collections.Generic;
using System.Text;

namespace Gadgetron.RatchetAndClank
{
    /// <summary>
    /// Represents a weapon in the game.
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// The address of the weapon
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The address in RAM in which the item check is performed.
        /// </summary>
        int CheckAddress { get; }
    }
}
