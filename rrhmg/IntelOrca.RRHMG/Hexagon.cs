using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace IntelOrca.RRHMG
{
	/// <summary>
	/// Represents a map cell hexagon that can contain child hexagons and be a child of a parent hexagon.
	/// </summary>
    public class Hexagon
    {
		/// <summary>
		/// The parent hexagon.
		/// </summary>
		public Hexagon Parent { get; set; }

		/// <summary>
		/// Child hexagons inside this one.
		/// </summary>
		public Hexagon[] Children { get; set; }

		/// <summary>
		/// The terrain information for this hexagon.
		/// </summary>
		public TerrainInfo TerrainInfo { get; set; }

		/// <summary>
		/// Gets the child hexagons from this hexagon's parent excluding this hexagon.
		/// </summary>
		public IEnumerable<Hexagon> Siblings
		{
			get
			{
				if (Parent == null)
					return Enumerable.Empty<Hexagon>();
				return Parent.Children.Where(x => x != this);
			}
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="Hexagon"/> class.
		/// </summary>
		/// <param name="terrainInfo">The terrain info.</param>
		public Hexagon(TerrainInfo terrainInfo)
		{
			TerrainInfo = terrainInfo;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="Hexagon"/> class.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="terrainInfo">The terrain info.</param>
		public Hexagon(Hexagon parent, TerrainInfo terrainInfo)
		{
			Parent = parent;
			TerrainInfo = terrainInfo;
		}

		#region Static methods

		/// <summary>
		/// The point size offsets
		/// </summary>
		public static Point[] PointSizeOffsets = new Point[] {
			new Point(-0.50,  0.0),
			new Point(-0.25, -0.5),
			new Point( 0.25, -0.5),
			new Point( 0.50,  0.0),
			new Point( 0.25,  0.5),
			new Point(-0.25,  0.5),
		};

		/// <summary>
		/// Gets the width of a hexagon given its size.
		/// </summary>
		/// <param name="size">The size of the hexagon.</param>
		public static double GetWidth(double size)
		{
			return size * 2.0;
		}

		/// <summary>
		/// Gets the height of a hexagon given its size.
		/// </summary>
		/// <param name="size">The size of the hexagon.</param>
		public static double GetHeight(double size)
		{
			return size * Math.Sqrt(3.0);
		}

		#endregion
	}
}
