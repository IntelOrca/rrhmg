using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace IntelOrca.RRHMG
{
	/// <summary>
	/// Represents a hexagon pattern.
	/// </summary>
	public class HexagonPattern
	{
		private readonly string _name;
		private readonly double _childSizeFactor;
		private readonly IReadOnlyList<Point> _offsets;

		/// <summary>
		/// Gets the name of the pattern.
		/// </summary>
		public string Name { get { return _name; } }

		/// <summary>
		/// Gets the ratio between the parent hexagon size and the child hexagon size.
		/// </summary>
		public double ChildSizeFactor { get { return _childSizeFactor; } }

		/// <summary>
		/// Gets the offsets in units of hexagon width / height for all child hexagons relative to the position of the parent
		/// hexagon.
		/// </summary>
		public IReadOnlyList<Point> Offsets { get { return _offsets; } }

		/// <summary>
		/// Initialises a new instance of the <see cref="HexagonPattern"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="offsets">The child hexagon offsets.</param>
		public HexagonPattern(string name, double childSizeFactor, IReadOnlyList<Point> offsets)
		{
			_name = name;
			_childSizeFactor = childSizeFactor;
			_offsets = offsets;
		}

		/// <summary>
		/// Definition of all the patterns.
		/// </summary>
		private static IDictionary<string, HexagonPattern> PaternDictionary = 
			new [] {
				new HexagonPattern("ThreeOverOne", 1.0 / 2.0, new Point[] {
					new Point(-0.25, -0.5),
					new Point(+0.50,  0.0),
					new Point(-0.25, +0.5),
					new Point(+0.50,  1.0)
				}),
				new HexagonPattern("6-flower", 1.0 / 3.0, new Point[] {
					new Point(-0.75, -0.5),
					new Point(-0.75,  0.5),
					new Point( 0.00, -1.0),
					new Point( 0.00,  0.0),
					new Point( 0.00,  1.0),
					new Point( 0.75, -0.5),
					new Point( 0.75,  0.5),
				})
			}.ToDictionary(x => x.Name);

		/// <summary>
		/// Gets all the defined <see cref="HexagonPattern">hexagon patterns</see>.
		/// </summary>
		public static IEnumerable<HexagonPattern> Patterns
		{
			get { return PaternDictionary.Values; }
		}

		/// <summary>
		/// Gets a pattern with the specified name.
		/// </summary>
		/// <param name="name">The name of the pattern.</param>
		/// <returns>A <see cref="HexagonPattern" />.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">Unknown name.</exception>
		public static HexagonPattern FromName(string name)
		{
			return PaternDictionary[name];
		}
	}
}
