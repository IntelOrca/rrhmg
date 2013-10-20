using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace IntelOrca.RRHMG
{
    public class Hexagon
    {
		public Hexagon Parent { get; set; }
		public Hexagon[] Children { get; set; }
		public Color Colour { get; set; }

		public IEnumerable<Hexagon> Siblings
		{
			get
			{
				if (Parent == null)
					return Enumerable.Empty<Hexagon>();
				return Parent.Children.Where(x => x != this);
			}
		}

		public Hexagon(Color colour)
		{
			Colour = colour;
		}

		public Hexagon(Hexagon parent, Color colour)
		{
			Parent = parent;
			Colour = colour;
		}

		#region Static methods

		public static Point[] PointSizeOffsets = new Point[] {
			new Point(-0.50,  0.0),
			new Point(-0.25, -0.5),
			new Point( 0.25, -0.5),
			new Point( 0.50,  0.0),
			new Point( 0.25,  0.5),
			new Point(-0.25,  0.5),
		};

		public static double GetWidth(double size)
		{
			return size * 2.0;
		}

		public static double GetHeight(double size)
		{
			return Math.Sqrt(3.0) / 2.0 * 2.0 * size;
		}

		#endregion
	}
}
