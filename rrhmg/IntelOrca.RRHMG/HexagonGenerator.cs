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
using System.ComponentModel;

namespace IntelOrca.RRHMG
{
    /// <summary>
    /// Base class for hexagon generators.
    /// </summary>
    abstract class HexagonGenerator
    {
        public const int InnerHexagonCount = 6;

        private readonly World _world;
        public World World { get { return _world; } }

        protected HexagonGenerator(World world)
        {
            _world = world;
        }

        /// <summary>
        /// Generates a starting point hexagon usually for the world's origin.
        /// </summary>
        /// <returns></returns>
        public Hexagon GenerateBaseHexagon()
        {
            return new Hexagon(
                World.GetNewId(),
                0,
                new Point(0.0, 0.0),
                1.0,
                Shape
            );
        }

        /// <summary>
        /// Generates InnerHexagonCount hexagons inside the specified parent hexagon.
        /// </summary>
        /// <param name="parent"></param>
        public void GenerateInnerHexagons(Hexagon parent)
        {
            for (int i = 0; i < InnerHexagonCount; i++) {
                // Create a new hexagon with initial properties and dimensions
                var hexagon = new Hexagon(
                    World.GetNewId(),
                    parent.Level + 1,
                    new Point(
                        parent.Location.X + (InnerOffsets[i].X * (parent.Size.Width / 2.0)),
                        parent.Location.Y + (InnerOffsets[i].Y * (parent.Size.Height / 2.0))
                    ),
                    parent.Radius / 2.0,
                    Shape
                );

                // Add the hexagon as a child of parent
                parent.AddChild(hexagon);
            }
        }

        /// <summary>
        /// Gets the hexagon shape of the hexagons that the generator creates.
        /// </summary>
        public abstract HexagonShape Shape { get; }

        /// <summary>
        /// Gets the offset mulipliers to position an inner hexagon inside an outer hexagon from the centre.
        /// </summary>
        protected abstract IList<Point> InnerOffsets { get; }

        /// <summary>
        /// Creates a new instance of the relevent hexagon generator of the specified hexagon
        /// shape.
        /// </summary>
        /// <param name="world"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static HexagonGenerator FromShape(World world, HexagonShape shape)
        {
            switch (shape) {
                case HexagonShape.PointTop:
                    return new PointTopHexagonGenerator(world);
                case HexagonShape.FlatTop:
                    return new FlatTopHexagonGenerator(world);
                default:
                    throw new InvalidEnumArgumentException("shape", (int)shape, typeof(HexagonShape));
            }
        }
    }
}
