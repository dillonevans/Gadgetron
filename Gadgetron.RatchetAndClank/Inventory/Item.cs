namespace Gadgetron.RatchetAndClank.Inventory
{
    /// <summary>
    /// An item in the game.
    /// </summary>
    public class Item : IInventoryItem
    {
        #region Fields

        public static readonly Item Hoverboard = new("Hoverboard", checkAddress: 0x2013D4DE);
        public static readonly Item Persuader = new("Persuader", checkAddress: 0x2013D4E3);
        public static readonly Item MapoMatic = new("Map-o-Matic", checkAddress: 0x2013D4E1);
        public static readonly Item BoltGrabber = new("Bolt Grabber", checkAddress: 0x2013D4E2);

        #endregion

        #region Properties

        /// <summary>
        /// Enumerates all items.
        /// </summary>
        public static IEnumerable<Item> Items => [Hoverboard, Persuader, MapoMatic, BoltGrabber];
        public string Name { get; private set; }
        public uint CheckAddress { get; private set; }

        #endregion

        #region Constructor

        private Item(string name, uint checkAddress)
        {
            this.Name = name;
            this.CheckAddress = checkAddress;
        }

        #endregion
    }
}
