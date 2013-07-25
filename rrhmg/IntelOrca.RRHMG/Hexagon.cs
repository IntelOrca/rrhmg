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

using System;
using System.Collections.Generic;

namespace IntelOrca.RRHMG
{
    /// <summary>
    /// Represents a single hexagon in the world that can have child hexagons.
    /// </summary>
    class Hexagon
    {
        /// <summary>
        /// Gets the unique identifier for the hexagon. Potentially wraps when over int.MaxValue
        /// hexagons have been created.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the recurse level within the hexagon tree. Can be negative or positive.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the location of the hexagon relative to the world.
        /// </summary>
        public Point Location { get; private set; }

        /// <summary>
        /// Gets the radius of the hexagon.
        /// </summary>
        public double Radius { get; private set; }

        /// <summary>
        /// Gets the shape of the hexagon.
        /// </summary>
        public HexagonShape Shape { get; private set; }

        /// <summary>
        /// Gets the terrain information for the hexagon such as the elevation, moisture etc.
        /// </summary>
        private Terrain _terrain;

        #region Hexagon References

        /// <summary>
        /// Gets the reference to the parent hexagon, if one exists.
        /// </summary>
        public Hexagon Parent { get; private set; }

        IEnumerable<Hexagon> Siblings
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<Hexagon> Surrounders
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerable<Hexagon> Children
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        /// <summary>
        /// Size of the hexagon relative to the world.
        /// </summary>
        public Size Size
        {
            get
            {
                double diameter = Radius * 2.0;
                double shorterDimension = Math.Sqrt(3.0) / 2.0 * diameter;

                switch (Shape) {
                    case HexagonShape.PointTop:
                        return new Size(shorterDimension, diameter);
                    case HexagonShape.FlatTop:
                        return new Size(diameter, shorterDimension);
                    default:
                        throw new HexagonException("Invalid hexagon shape");
                }
            }
        }

        public Hexagon(int id, int level, Point location, double radius, HexagonShape shape)
        {
            Id = id;
            Level = level;
            Location = location;
            Radius = radius;
            Shape = shape;
        }

        public void AddChild(Hexagon hexagon)
        {
            throw new NotImplementedException();
        }
    }
}
