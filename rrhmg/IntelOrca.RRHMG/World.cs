#region RRHMG, © Copyright Ted John 2013
// Recursive random hexagon map generator
// Intelorca.RRHMG
// 
// Universiy of Manchester, Computer Science
// Third year project
//
// Application to generate random hexagonal maps of terrain where the map can recurse indefinitely
// in the same manner as a fractal.
// 
// © Copyright Ted John 2013
#endregion

namespace IntelOrca.RRHMG
{
    /// <summary>
    /// Represents a world of hexagons.
    /// </summary>
    class World
    {
        /// <summary>
        /// Shape of the hexagons that make up the world. This can not be changed and is therefore
        /// fixed to this world forever.
        /// </summary>
        private readonly HexagonShape _shape;

        /// <summary>
        /// Renderer instance for rendering the world with a viewport perspective.
        /// </summary>
        private readonly WorldRenderer _renderer;

        /// <summary>
        /// Hexagon generator instance.
        /// </summary>
        private readonly HexagonGenerator _generator;
        
        /// <summary>
        /// A current id that is used and incremented for every new hexagon.
        /// </summary>
        private int _nextHexagonId = 0;

        /// <summary>
        /// The origin of the world, the first initial hexagon.
        /// </summary>
        public Hexagon Origin { get; private set; }

        /// <summary>
        /// Constructs a new world with a particular hexagon shape.
        /// </summary>
        /// <param name="shape"></param>
        public World(HexagonShape shape)
        {
            _shape = shape;
            _renderer = new WorldRenderer(this);
            _generator = HexagonGenerator.FromShape(this, _shape);

            Origin = _generator.GenerateBaseHexagon();
        }

        /// <summary>
        /// Gets the next id that a new hexagon should use.
        /// </summary>
        /// <returns></returns>
        public int GetNewId()
        {
            return _nextHexagonId++;
        }

        /// <summary>
        /// Renders the current world from the specified viewport's perspective.
        /// </summary>
        /// <param name="viewport"></param>
        public void Render(Viewport viewport)
        {
            _renderer.Render(viewport);
        }
    }
}