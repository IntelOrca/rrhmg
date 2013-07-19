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