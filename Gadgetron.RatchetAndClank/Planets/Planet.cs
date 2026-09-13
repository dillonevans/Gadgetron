namespace Gadgetron.RatchetAndClank.Planets
{
    /// <summary>
    /// A planet in the game.
    /// </summary>
    public class Planet
    {
        #region Fields

        public static readonly Planet VeldinTutorial = new("Veldin (Tutorial)", id: 0);
        public static readonly Planet Novalis = new("Novalis", id: 1);
        public static readonly Planet Aridia = new("Aridia", id: 2);
        public static readonly Planet Kerwan = new("Kerwan", id: 3);
        public static readonly Planet Eudora = new("Eudora", id: 4);
        public static readonly Planet Rilgar = new("Rilgar", id: 5);
        public static readonly Planet NebulaG34 = new("Nebula G34", id: 6);
        public static readonly Planet Umbris = new("Umbris", id: 7);
        public static readonly Planet Batalia = new("Batalia", id: 8);
        public static readonly Planet Gaspar = new("Gaspar", id: 9);
        public static readonly Planet Orxon = new("Orxon", id: 10);
        public static readonly Planet Pokitaru = new("Pokitaru", id: 11);
        public static readonly Planet Hoven = new("Hoven", id: 12);
        public static readonly Planet OltanisOrbit = new("Oltanis Orbit", id: 13);
        public static readonly Planet Quartu = new("Quartu", id: 14);
        public static readonly Planet KaleboIII = new("Kalebo III", id: 15);
        public static readonly Planet Veldin = new("Veldin", id: 16);

        #endregion

        #region Properties

        /// <summary>
        /// Enumerates all planets.
        /// </summary>
        public static IEnumerable<Planet> Planets =>
        [
            VeldinTutorial,
            Novalis,
            Aridia,
            Kerwan,
            Eudora,
            Rilgar,
            NebulaG34,
            Umbris,
            Batalia,
            Gaspar,
            Orxon,
            Pokitaru,
            Hoven,
            OltanisOrbit,
            Quartu,
            KaleboIII,
            Veldin
        ];

        public string Name { get; private set; }
        public int Id { get; private set;  }

        #endregion

        #region Constructor

        private Planet(string name, int id)
        {
            this.Name = name;
            this.Id = id;
        }

        #endregion
    }
}
