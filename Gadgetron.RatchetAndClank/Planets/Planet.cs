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
        public static readonly Planet Oltanis = new("Oltanis", id: 14);
        public static readonly Planet Quartu = new("Quartu", id: 15);
        public static readonly Planet KaleboIII = new("Kalebo III", id: 16);
        public static readonly Planet VeldinOrbit = new("Veldin Orbit", id: 17);
        public static readonly Planet Veldin = new("Veldin", id: 18);

        private static readonly Planet[] planets =
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
            Oltanis,
            OltanisOrbit,
            Quartu,
            KaleboIII,
            VeldinOrbit,
            Veldin
        ];

        private static readonly Dictionary<int, Planet> planetLookup = planets.ToDictionary(static planet => planet.Id);

        #endregion

        #region Properties

        /// <summary>
        /// The name of the planet.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The unique identifier of the planet.
        /// </summary>
        public int Id { get; private set;  }

        /// <summary>
        /// The collection of all planets.
        /// </summary>
        public static IReadOnlyCollection<Planet> Planets => planets;

        #endregion

        #region Constructor

        private Planet(string name, int id)
        {
            this.Name = name;
            this.Id = id;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the <see cref="Planet"/> with the corresponding <paramref name="id"/>, or <see langword="null"/>
        /// if it cannot be found.
        /// </summary>
        /// <param name="id">The Id of the planet.</param>
        /// <returns>The <see cref="Planet"/> instance, or <see langword="null"/>.</returns>
        public static Planet? GetById(int id)
        {
            if (!planetLookup.TryGetValue(id, out Planet? planet))
            {
                return null;
            }

            return planet;
        }

        #endregion
    }
}
