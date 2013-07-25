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
    class WorldRenderer
    {
        private readonly HexagonRenderer _hexagonRenderer = new HexagonRenderer();
        private readonly World _world;

        public WorldRenderer(World world)
        {
            _world = world;
        }

        public void Render(Viewport viewport)
        {
            _hexagonRenderer.Render(viewport, _world.Origin);
        }
    }
}
