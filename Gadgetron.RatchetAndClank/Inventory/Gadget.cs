namespace Gadgetron.RatchetAndClank.Inventory
{
    /// <summary>
    /// A gadget in the game.
    /// </summary>
    public class Gadget : IInventoryItem
    {
        #region Fields

        public static readonly Gadget Grindboots = new("Grindboots", checkAddress: 0x2013D4DD);
        public static readonly Gadget HeliPack = new("Heli-pack", checkAddress: 0x2013D4C2);
        public static readonly Gadget Hydrodisplacer = new("Hydrodisplacer", checkAddress: 0x2013D4D6);
        public static readonly Gadget Hologuise = new("Hologuise", checkAddress: 0x2013D4DF);
        public static readonly Gadget HydroPack = new("Hydro-Pack", checkAddress: 0x2013D4C4);
        public static readonly Gadget Magneboots = new("Magneboots", checkAddress: 0x2013D4DC);
        public static readonly Gadget MetalDetector = new("Metal Detector", checkAddress: 0x2013D4DB);
        public static readonly Gadget O2Mask = new("O2 Mask", checkAddress: 0x2013D4C6);
        public static readonly Gadget Pda = new("PDA", checkAddress: 0x2013D4E0);
        public static readonly Gadget PilotsHelmet = new("Pilot's Helmet", checkAddress: 0x2013D4C7);
        public static readonly Gadget SonicSummoner = new("Sonic Summoner", checkAddress: 0x2013D4C5);
        public static readonly Gadget Swingshot = new("Swingshot", checkAddress: 0x2013D4CC);
        public static readonly Gadget ThrusterPack = new("Thruster-Pack", checkAddress: 0x2013D4C3);
        public static readonly Gadget Tresspasser = new("Tresspasser", checkAddress: 0x2013D4DA);

        #endregion

        #region Properties

        /// <summary>
        /// Enumerates all gadgets.
        /// </summary>
        public static IEnumerable<Gadget> Gadgets =>
        [
            Grindboots,
            HeliPack,
            Hydrodisplacer,
            Hologuise,
            HydroPack,
            Magneboots,
            MetalDetector,
            O2Mask,
            PilotsHelmet,
            Pda,
            SonicSummoner,
            Swingshot,
            ThrusterPack,
            Tresspasser
        ];

        /// <summary>
        /// The name of the gadget.
        /// </summary>
        public string Name { get; private set; }

        public uint CheckAddress { get; private set; }

        #endregion

        #region Constructor

        private Gadget(string name, uint checkAddress)
        {
            this.Name = name;
            this.CheckAddress = checkAddress;
        }

        #endregion
    }
}
