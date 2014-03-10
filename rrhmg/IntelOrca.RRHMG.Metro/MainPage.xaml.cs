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
		private readonly Random _random = new Random(0);
		private readonly DispatcherTimer _demonstrationTimer = new DispatcherTimer();

		private readonly MediaElement _mediaElement = new MediaElement();

		public static HexagonPattern HexagonPattern { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MainPage"/> class.
		/// </summary>
        public MainPage()
        {
            this.InitializeComponent();

			_demonstrationTimer.Interval = TimeSpan.FromMilliseconds(100);
			_demonstrationTimer.Tick += async (s, e) => {
				_mediaElement.SetSource(await Util.GetBeepStream(200, 800, 100), string.Empty);
				_mediaElement.Play();
				
				NextDemonstrationAction();
				XamlHexagonMap.GenerateHexagonShapes();
				UpdateStatistics();
			};

			// Start showing hexagon level
			XamlHexagonMap.ShowingHexagon = new Hexagon(new TerrainInfo());
			XamlHexagonMap.HexagonPattern = HexagonPattern == null ? HexagonPattern.Patterns.First() : HexagonPattern;

			// Extra events
			Window.Current.CoreWindow.KeyDown += OnKeyDown;
        }

		/// <summary>
		/// Finds a random hexagon with no children to generate subchildren for.
		/// </summary>
		/// <param name="hexagon">The root hexagon.</param>
		/// <returns>True if child hexagons were successfully generated for a random hexagon.</returns>
		private bool NextDemonstrationAction(Hexagon hexagon = null, int depth = 0)
		{
			if (depth > 4)
				return false;

			if (hexagon == null)
				hexagon = XamlHexagonMap.ShowingHexagon;

			// Generate children if there aren't any and finish
			if (hexagon.Children == null) {
				hexagon.GenerateChildren(_random, XamlHexagonMap.HexagonPattern);
				_numHexagonsGenerated += hexagon.Children.Length;
				return true;
			}

			// Enumerate children in a random order
			foreach (Hexagon child in hexagon.Children.OrderBy(x => _random.Next()))
				if (NextDemonstrationAction(child, depth + 1))
					return true;

			return false;
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

			_numHexagonsGenerated += numChildrenGenerated;
			UpdateStatistics();
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
				if (e.Hexagon.Children == null) {
					e.Hexagon.GenerateChildren(_random, XamlHexagonMap.HexagonPattern);
					_numHexagonsGenerated += e.Hexagon.Children.Length;
					UpdateStatistics();
				}
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

		private void DemonstrationButtonOnClick(object sender, RoutedEventArgs e)
		{
			var button = (AppBarButton)sender;
			if (!_demonstrationTimer.IsEnabled) {
				_demonstrationTimer.Start();
				button.Icon = new SymbolIcon(Symbol.Stop);
			} else {
				_demonstrationTimer.Stop();
				button.Icon = new SymbolIcon(Symbol.Play);
			}
		}

		private void RestartButtonOnClick(object sender, RoutedEventArgs e)
		{
			var hexagonPatterns = HexagonPattern.Patterns.ToArray();
			var hexagonPattern = hexagonPatterns[_random.Next(hexagonPatterns.Length)];
			hexagonPattern = hexagonPatterns[0];

			XamlHexagonMap.ShowingHexagon = new Hexagon(new TerrainInfo());
			XamlHexagonMap.HexagonPattern = hexagonPattern;

			BottomAppBar.IsOpen = false;
		}

		private void SelectPatternOnClick(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(PatternSelectionPage), this);
		}

		private int _numHexagonsGenerated;

		private void UpdateStatistics()
		{
			UpdateStatistics(new KeyValuePair<string, object>[] {
				new KeyValuePair<string, object>("Generated hexagons", _numHexagonsGenerated),
				new KeyValuePair<string, object>("Levels", XamlHexagonMap.ShowingHexagon.Levels)
			});
		}

		private void UpdateStatistics(IEnumerable<KeyValuePair<string, object>> statisticsEnumerable)
		{
			KeyValuePair<string, object>[] statistics = statisticsEnumerable.ToArray();
			var children = XamlStatisticsContainer.Children.ToArray();

			// Remove extra textblocks
			while (XamlStatisticsContainer.Children.Count > statistics.Length)
				XamlStatisticsContainer.Children.RemoveAt(XamlStatisticsContainer.Children.Count - 1);

			// Add textblocks
			while (XamlStatisticsContainer.Children.Count < statistics.Length) {
				XamlStatisticsContainer.Children.Add(new TextBlock() {
					Style = (Style)Resources["StatisticTextBlockStyle"]
				});
			}

			// Set textblocks
			int index = 0;
			foreach (TextBlock textBlock in XamlStatisticsContainer.Children) {
				textBlock.Text = String.Format("{0}: {1}", statistics[index].Key, statistics[index].Value);
				index++;
			}
		}
	}
}
