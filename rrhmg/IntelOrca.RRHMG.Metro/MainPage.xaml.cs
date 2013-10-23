using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace IntelOrca.RRHMG.Metro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class MainPage : Page
    {
		private Random _random = new Random(0);
		private int _maxLevelsToShow = 5;

		private Hexagon _headHexagon = new Hexagon(new TerrainInfo());
		private Hexagon _showingHexagon;

		/// <summary>
		/// Gets the centre position of the map canvas.
		/// </summary>
		public Point CanvasCentrePosition
		{
			get { return new Point(XamlCanvas.ActualWidth / 2.0, XamlCanvas.ActualHeight / 2.0); }
		}

        public MainPage()
        {
			_showingHexagon = _headHexagon;

            this.InitializeComponent();

			this.Loaded += (s, e) => GenerateHexagons();
        }

		protected override void OnKeyDown(KeyRoutedEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.Key == VirtualKey.H) {
				int numChildrenGenerated = 0;
				int level = 1;
				List<Hexagon> nextLevelOfHexagons =
					_showingHexagon.Children == null ?
						new[] { _showingHexagon }.ToList() :
						_showingHexagon.Children.ToList();

				do {
					// Obtain the current level of hexagons and clear the list for the next level
					var levelOfHexagonsPass = nextLevelOfHexagons.ToArray();
					nextLevelOfHexagons.Clear();

					// For each hexagon on this level
					foreach (Hexagon hexagon in levelOfHexagonsPass) {
						// Generate some children if there aren't any
						if (hexagon.Children == null) {
							hexagon.GenerateChildren(_random);
							numChildrenGenerated += hexagon.Children.Length;
						}

						// Add the children to the next level of hexagons list
						nextLevelOfHexagons.AddRange(hexagon.Children);
					}

					// Increment current level
					level++;
				} while (numChildrenGenerated == 0 && level < _maxLevelsToShow);

				if (numChildrenGenerated > 0)
					GenerateHexagons();
			}
		}

		private void GenerateHexagons()
		{
			// Calculate the the hexagon size to cover the entire canvas
			double size = Math.Max(XamlCanvas.ActualWidth, XamlCanvas.ActualHeight) * 0.75;

			// Clear the canvas and regenerate the hexagons
			XamlCanvas.Children.Clear();
			foreach (HexagonShape hex in GenerateHexagon(size, CanvasCentrePosition, _maxLevelsToShow, _showingHexagon))
				XamlCanvas.Children.Add(hex);

			this.Focus(Windows.UI.Xaml.FocusState.Keyboard);
		}

		private IEnumerable<HexagonShape> GenerateHexagon(double size, Point position, int levels, Hexagon hexagon)
		{
			// Base case
			if (levels < 0)
				yield break;

			// The child hexagon size calculation
			double nextSize = size / 3.0;

			// Get the width and height for this the next size down of this hexagon
			double hexWidth = Hexagon.GetWidth(nextSize);
			double hexHeight = Hexagon.GetHeight(nextSize);

			// Multiply all the child offsets by the calculated hexagon width / height
			var offsets = Hexagon.ChildOffsets.Select(x => new Point(x.X * hexWidth, x.Y * hexHeight)).ToArray();

			// Do not draw any more child hexagons if we have reached the maximum display level or there are no children
			if (levels > 0 && hexagon.Children != null) {
				for (int i = 0; i < hexagon.Children.Length; i++) {
					Point offset = offsets[i];

					// Recursively generate child hexagon shapes
					var hexagonShapes = GenerateHexagon(
						nextSize,
						new Point(position.X + offset.X, position.Y + offset.Y),
						levels - 1,
						hexagon.Children[i]
					);

					// Return all the child hexagon shapes
					foreach (HexagonShape hex in hexagonShapes)
						yield return hex;
				}
			} else {
				// Return a single hexagon shape for this current hexagon
				yield return GenerateHexagon(size, position, hexagon);
			}
		}

		private HexagonShape GenerateHexagon(double size, Point position, Hexagon hexagon)
		{
			// Create the hexagon shape associated with this hexagon
			var hex = new HexagonShape(hexagon, size);

			// Set the position of the hexagon in the cavas container
			Canvas.SetLeft(hex, position.X - (hex.Width / 2.0));
			Canvas.SetTop(hex, position.Y - (hex.Height / 2.0));

			// Set their tap events
			hex.Tapped += HexagonOnTapped;
			hex.RightTapped += HexagonOnRightTapped;

			return hex;
		}

		void HexagonOnTapped(object sender, TappedRoutedEventArgs e)
		{
			var tappedHexagonShape = sender as HexagonShape;
			var tappedHexagon = tappedHexagonShape.Hexagon;

			// Find the number of levels down this hexagon is from the top level showing hexagon
			int levelsDown = 1;
			var nextLevelHexagon = tappedHexagon;
			while (nextLevelHexagon.Parent != _showingHexagon && nextLevelHexagon.Parent != null) {
				nextLevelHexagon = nextLevelHexagon.Parent;
				levelsDown++;
			}

			// Check if the tapped hexagon's children are too deep to see
			if (levelsDown < _maxLevelsToShow) {
				// Generate children for the tapped hexagon
				if (tappedHexagon.Children == null)
					tappedHexagon.GenerateChildren(_random);
			} else {
				// Zoom in by one level
				_showingHexagon = nextLevelHexagon;
			}

			// Redisplay
			GenerateHexagons();
		}

		void HexagonOnRightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			// If there is a parent, zoom out to the parent level
			if (_showingHexagon.Parent != null) {
				_showingHexagon = _showingHexagon.Parent;
				GenerateHexagons();
			}
		}
    }
}
