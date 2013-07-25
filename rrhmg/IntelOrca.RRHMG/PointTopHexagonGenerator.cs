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

using System.Collections.Generic;

namespace IntelOrca.RRHMG
{
    class PointTopHexagonGenerator : HexagonGenerator
    {
        /// <summary>
        /// Represents the offset mulipliers to position an inner hexagon inside an outer hexagon from the centre.
        /// </summary>
        private readonly static Point[] InnerOffsetsArray = new [] {
            new Point(0.0     , -1.0/4.0),
            new Point(-1.0/4.0, -1.0/8.0), new Point(+1.0/4.0, -1.0/8.0),
            new Point(-1.0/4.0, +1.0/8.0), new Point(+1.0/4.0, +1.0/8.0),
            new Point(0.0     , +1.0/4.0)
        };

        public PointTopHexagonGenerator(World world)
            : base(world)
        {
        }

        public override HexagonShape Shape
        {
            get { return HexagonShape.PointTop; }
        }

        protected override IList<Point> InnerOffsets
        {
            get { return InnerOffsetsArray; }
        }
    }
}