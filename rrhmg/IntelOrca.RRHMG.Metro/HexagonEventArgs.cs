using Windows.UI.Xaml;

namespace IntelOrca.RRHMG.Metro
{
	/// <summary>
	/// Represents the event arguments for a hexagon event in a <see cref="HexagonMap"/>.
	/// </summary>
	public sealed class HexagonEventArgs
	{
		private readonly RoutedEventArgs _originalEvent;
		private readonly HexagonShape _hexagonShape;
		private readonly int _levelDepth;

		/// <summary>
		/// Gets the original event arguments that trigged this event.
		/// </summary>
		public RoutedEventArgs OriginalEvent { get { return _originalEvent; } }

		/// <summary>
		/// Gets the <see cref="HexagonShape"/> source.
		/// </summary>
		public HexagonShape HexagonShape { get { return _hexagonShape; } }

		/// <summary>
		/// Gets the <see cref="Hexagon"/> that the source <see cref="HexagonShape"/> represents.
		/// </summary>
		public Hexagon Hexagon { get { return _hexagonShape.Hexagon; } }

		/// <summary>
		/// Gets the number of levels between the top level showing hexagon and the source hexagon.
		/// </summary>
		public int LevelDepth { get { return _levelDepth; } }

		/// <summary>
		/// Gets or sets whether this event has been handled or not.
		/// </summary>
		public bool Handled { get; set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="HexagonEventArgs"/> class.
		/// </summary>
		/// <param name="originalEvent">The original <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		/// <param name="hexagonShape">The source hexagon shape.</param>
		/// <param name="levelDepth">The number of levels between the top level showing hexagon and the source hexagon.</param>
		public HexagonEventArgs(
			RoutedEventArgs originalEvent,
			HexagonShape hexagonShape,
			int levelDepth
		)
		{
			_originalEvent = originalEvent;
			_hexagonShape = hexagonShape;
			_levelDepth = levelDepth;
		}
	}
}
