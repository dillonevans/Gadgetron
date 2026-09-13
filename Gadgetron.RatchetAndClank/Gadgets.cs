namespace Gadgetron.RatchetAndClank
{
    /// <summary>
    /// Every gadget in the game.
    /// </summary>
    public static class Gadgets
    {
        #region Records

        private record Gadget(string Name, int CheckAddress) : IGadget;

        #endregion

        #region Fields

        public static readonly IGadget BoltGrabber = new Gadget(GadgetNames.BoltGrabber, CheckAddress: 0x2013D4E2);
        public static readonly IGadget Grindboots = new Gadget(GadgetNames.Grindboots, CheckAddress: 0x2013D4DD);
        public static readonly IGadget HeliPack = new Gadget(GadgetNames.HeliPack, CheckAddress: 0x2013D4C2);
        public static readonly IGadget Hydrodisplacer = new Gadget(GadgetNames.Hydrodisplacer, CheckAddress: 0x2013D4D6);
        public static readonly IGadget Hologuise = new Gadget(GadgetNames.Hologuise, CheckAddress: 0x2013D4DF);
        public static readonly IGadget Hoverboard = new Gadget(GadgetNames.Hoverboard, CheckAddress: 0x2013D4DE);
        public static readonly IGadget HydroPack = new Gadget(GadgetNames.HydroPack, CheckAddress: 0x2013D4C4);
        public static readonly IGadget Magneboots = new Gadget(GadgetNames.Magneboots, CheckAddress: 0x2013D4DC);
        public static readonly IGadget MapoMatic = new Gadget(GadgetNames.MapoMatic, CheckAddress: 0x2013D4E1);
        public static readonly IGadget MetalDetector = new Gadget(GadgetNames.MetalDetector, CheckAddress: 0x2013D4DB);
        public static readonly IGadget O2Mask = new Gadget(GadgetNames.O2Mask, CheckAddress: 0x2013D4C6);
        public static readonly IGadget Pda = new Gadget(GadgetNames.Pda, CheckAddress: 0x2013D4E0);
        public static readonly IGadget Persuader = new Gadget(GadgetNames.Persuader, CheckAddress: 0x2013D4E3);
        public static readonly IGadget PilotsHelmet = new Gadget(GadgetNames.PilotsHelmet, CheckAddress: 0x2013D4C7);
        public static readonly IGadget SonicSummoner = new Gadget(GadgetNames.SonicSummoner, CheckAddress: 0x2013D4C5);
        public static readonly IGadget Swingshot = new Gadget(GadgetNames.Swingshot, CheckAddress: 0x2013D4CC);
        public static readonly IGadget ThrusterPack = new Gadget(GadgetNames.ThrusterPack, CheckAddress: 0x2013D4C3);
        public static readonly IGadget Tresspasser = new Gadget(GadgetNames.Tresspasser, CheckAddress: 0x2013D4DA);

        #endregion

        #region Properties

        /// <summary>
        /// Enumerates all gadgets.
        /// </summary>
        public static IEnumerable<IGadget> All =>
        [
            BoltGrabber,
            Grindboots,
            HeliPack,
            Hydrodisplacer,
            Hologuise,
            Hoverboard,
            HydroPack,
            Magneboots,
            MapoMatic,
            MetalDetector,
            O2Mask,
            PilotsHelmet,
            Pda,
            Persuader,
            SonicSummoner,
            Swingshot,
            ThrusterPack,
            Tresspasser
        ];

        #endregion
    }
}
