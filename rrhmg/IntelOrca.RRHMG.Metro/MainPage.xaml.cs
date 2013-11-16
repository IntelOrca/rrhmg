using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IntelOrca.RRHMG.Metro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class MainPage : Page
    {
		private Random _random = new Random(0);

		/// <summary>
		/// Initializes a new instance of the <see cref="MainPage"/> class.
		/// </summary>
        public MainPage()
        {
            this.InitializeComponent();

			// Start showing hexagon level
			XamlHexagonMap.ShowingHexagon = new Hexagon(new TerrainInfo());

			// Extra events
			Window.Current.CoreWindow.KeyDown += OnKeyDown;
        }

		/// <summary>
		/// Find the first level that contains a hexagon without children and generate any child hexagons for hexagons that
		/// don't have children on that level.
		/// </summary>
		private void RecurseNextHexagonLevel()
		{
			int numChildrenGenerated = 0;
			int level = 0;
			
			// The top level is the showing hexagon
			var nextLevelOfHexagons = new List<Hexagon>();
			nextLevelOfHexagons.Add(XamlHexagonMap.ShowingHexagon);

			do {
				// Obtain the current level of hexagons and clear the list for the next level
				var levelOfHexagonsPass = nextLevelOfHexagons.ToArray();
				nextLevelOfHexagons.Clear();

				// For each hexagon on this level
				foreach (Hexagon hexagon in levelOfHexagonsPass) {
					// Generate some children if there aren't any
					if (hexagon.Children == null) {
						hexagon.GenerateChildren(_random, XamlHexagonMap.HexagonPattern);
						numChildrenGenerated += hexagon.Children.Length;
					}

					// Add the children to the next level of hexagons list
					nextLevelOfHexagons.AddRange(hexagon.Children);
				}

				// Increment current level
				level++;
			} while (numChildrenGenerated == 0 && level < XamlHexagonMap.MaxLevelsToShow);

			// Only bother regenerating if hexagons have been added
			if (numChildrenGenerated > 0)
				XamlHexagonMap.GenerateHexagonShapes();
		}

		/// <summary>
		/// Event handler for when a hexagon is tapped.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="HexagonEventArgs"/> instance containing the event data.</param>
		private void HexagonOnTapped(object sender, HexagonEventArgs e)
		{
			// Check if the tapped hexagon's children are too deep to see
			if (e.LevelDepth < XamlHexagonMap.MaxLevelsToShow) {
				// Generate children for the tapped hexagon
				if (e.Hexagon.Children == null)
					e.Hexagon.GenerateChildren(_random, XamlHexagonMap.HexagonPattern);
				e.Handled = true;
			}
		}

		/// <summary>
		/// Event handler for when a hexagon is tapped with the right button.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="HexagonEventArgs"/> instance containing the event data.</param>
		private void HexagonOnRightTapped(object sender, HexagonEventArgs e)
		{
		}

		/// <summary>
		/// Called before the KeyDown event occurs.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The data for the event.</param>
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.VirtualKey == VirtualKey.H)
				RecurseNextHexagonLevel();
		}

		private void RestartButtonOnClick(object sender, RoutedEventArgs e)
		{
			var hexagonPatterns = HexagonPattern.Patterns.ToArray();
			var hexagonPattern = hexagonPatterns[_random.Next(hexagonPatterns.Length)];

			XamlHexagonMap.ShowingHexagon = new Hexagon(new TerrainInfo());
			XamlHexagonMap.HexagonPattern = hexagonPattern;

			BottomAppBar.IsOpen = false;
		}
    }
}
