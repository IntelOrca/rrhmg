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
    /// Represents a particular hexagon shape.
    /// </summary>
    enum HexagonShape
    {
        /// <summary>
        /// Represents a hexagon with a pointed top.
        /// </summary>
        PointTop,

        /// <summary>
        /// Represents a hexagon with a flat edge top.
        /// </summary>
        FlatTop,
    }
}