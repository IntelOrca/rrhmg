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
    enum Biome
    {
        Temperate,
        Savannah,
        Tundra,
    }

    enum TerrainSurface
    {
        Grass,
        Snow,
        Ice,
    }

    /// <summary>
    /// Represens the terrain of a hexagon which includes properties such as elevation, moisture etc.
    /// </summary>
    class Terrain
    {
        public Biome Biome { get; set; }
        public Biome TerrainSurface { get; set; }
        public double Elevation { get; set; }
        public double Moisture { get; set; }
    }
}