namespace Gadgetron.RatchetAndClank.Inventory
{
    /// <summary>
    /// Represents an item in Ratchet's inventory.
    /// </summary>
    public interface IInventoryItem
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The address in RAM in which the item check is performed.
        /// </summary>
        uint CheckAddress { get; }
    }
}
