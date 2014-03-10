using System;
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
		/// <summary>
		/// Represents information about a child hexagon.
		/// </summary>
		public class ChildInfo
		{
			private readonly Point _offset;
			private readonly IReadOnlyCollection<int> _parentInfluences;

			/// <summary>
			/// Gets the offset in units of hexagon width / height for the child hexagon relative to the position of the parent
			/// hexagon.
			/// </summary>
			public Point Offset { get { return _offset; } }

			/// <summary>
			/// Gets the indicies of the parent hexagon's siblings that influence this child.
			/// </summary>
			public IReadOnlyCollection<int> ParentInfluences { get { return _parentInfluences; } }

			/// <summary>
			/// Initialises a new instance of the <see cref="ChildInfo"/> class.
			/// </summary>
			/// <param name="offset">The offset.</param>
			public ChildInfo(Point offset) : this(offset, Enumerable.Empty<int>()) { }

			/// <summary>
			/// Initialises a new instance of the <see cref="ChildInfo"/> class.
			/// </summary>
			/// <param name="offset">The offset.</param>
			/// <param name="parentInfluences">The parent influences.</param>
			public ChildInfo(Point offset, IEnumerable<int> parentInfluences)
			{
				_offset = offset;
				_parentInfluences = parentInfluences.ToArray();
			}

			/// <summary>
			/// Returns a <see cref="System.String" /> that represents this instance.
			/// </summary>
			/// <returns>
			/// A <see cref="System.String" /> that represents this instance.
			/// </returns>
			public override string ToString()
			{
				return String.Format("({0}, {1}) by [{2}]", _offset.X, _offset.Y, String.Join(", ", _parentInfluences));
			}
		}

		private readonly string _name;
		private readonly double _childSizeFactor;
		private readonly IReadOnlyList<ChildInfo> _childrenInfo;

		/// <summary>
		/// Gets the name of the pattern.
		/// </summary>
		public string Name { get { return _name; } }

		/// <summary>
		/// Gets the ratio between the parent hexagon size and the child hexagon size.
		/// </summary>
		public double ChildSizeFactor { get { return _childSizeFactor; } }

		/// <summary>
		/// Gets the children information such as offset and influences.
		/// </summary>
		public IReadOnlyList<ChildInfo> ChildrenInfo { get { return _childrenInfo; } }

		/// <summary>
		/// Initialises a new instance of the <see cref="HexagonPattern"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="childrenInfo">The child hexagon offsets.</param>
		public HexagonPattern(string name, double childSizeFactor, IReadOnlyList<ChildInfo> childrenInfo)
		{
			_name = name;
			_childSizeFactor = childSizeFactor;
			_childrenInfo = childrenInfo;
		}

		/// <summary>
		/// Definition of all the patterns.
		/// </summary>
		private static IDictionary<string, HexagonPattern> PaternDictionary = 
			new [] {
				new HexagonPattern("8-flower", 1.0 / 3.0, new ChildInfo[] {
					new ChildInfo(new Point(-0.75, -0.5)),
					new ChildInfo(new Point(-0.75,  0.5)),

					new ChildInfo(new Point( 0.00, -1.0)),
					new ChildInfo(new Point( 0.00,  0.0)),
					new ChildInfo(new Point( 0.00,  1.0)),
					
					new ChildInfo(new Point( 0.75, -0.5)),
					new ChildInfo(new Point( 0.75,  0.5)),

					new ChildInfo(new Point( 0.75,  1.5), new int[] { 3, 4, 6 }),
					new ChildInfo(new Point( 1.50,  0.0), new int[] { 3, 5, 6 })
				}),
				new HexagonPattern("ThreeOverOne", 1.0 / 2.0, new ChildInfo[] {
					new ChildInfo(new Point(-0.25, -0.5)),
					new ChildInfo(new Point(+0.50,  0.0)),
					new ChildInfo(new Point(-0.25, +0.5)),
					new ChildInfo(new Point(+0.50,  1.0), new int[] { 0, 1, 2 })
				}),
				new HexagonPattern("6-flower", 1.0 / 3.0, new ChildInfo[] {
					new ChildInfo(new Point(-0.75, -0.5)),
					new ChildInfo(new Point(-0.75,  0.5)),
					new ChildInfo(new Point( 0.00, -1.0)),
					new ChildInfo(new Point( 0.00,  0.0)),
					new ChildInfo(new Point( 0.00,  1.0)),
					new ChildInfo(new Point( 0.75, -0.5)),
					new ChildInfo(new Point( 0.75,  0.5)),
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
