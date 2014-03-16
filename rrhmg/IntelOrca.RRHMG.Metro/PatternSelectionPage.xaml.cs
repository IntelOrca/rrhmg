using IntelOrca.RRHMG.Metro.Common;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace IntelOrca.RRHMG.Metro
{
	/// <summary>
	/// A page that displays a collection of item previews.  In the Split Application this page
	/// is used to display and select one of the available groups.
	/// </summary>
	public sealed partial class PatternSelectionPage : Page
	{
		private NavigationHelper _navigationHelper;
	
		/// <summary>
		/// NavigationHelper is used on each page to aid in navigation and process lifetime management
		/// </summary>
		public NavigationHelper NavigationHelper { get { return this._navigationHelper; } }

		/// <summary>
		/// Initialises a new instance of the <see cref="PatternSelectionPage"/> class.
		/// </summary>
		public PatternSelectionPage()
		{
			this.InitializeComponent();
			this._navigationHelper = new NavigationHelper(this);
			this._navigationHelper.LoadState += navigationHelper_LoadState;
		}

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also provided when recreating a page
		/// from a prior session.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and a dictionary of state
		/// preserved by this page during an earlier session.  The state will be null the first time a page is visited.
		/// </param>
		private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
			foreach (HexagonPattern hexagonPattern in HexagonPattern.Patterns)
				itemGridView.Items.Add(GetItem(hexagonPattern));
		}

		/// <summary>
		/// Gets a UI item representing a hexagon pattern.
		/// </summary>
		/// <param name="pattern">The hexagon pattern.</param>
		/// <returns>A new <see cref="UIElement"/>.</returns>
		private UIElement GetItem(HexagonPattern pattern)
		{
			// Item is a stack panel
			var sp = new StackPanel() {
				Width = 256,
				Height = 256,
				Orientation = Orientation.Vertical,
				Background = new SolidColorBrush(Color.FromArgb(255, 96, 96, 96))
			};
			sp.Tapped += (s, e) => {
				if (MainPage.HexagonPattern != pattern) {
					MainPage.HexagonPattern = pattern;
					MainPage.HexagonSaved = null;
				}
				_navigationHelper.GoBack();
			};

			// Hexagon preview is in a canvas
			var canvas = new Canvas() {
				Width = sp.Width,
				Height = sp.Height - 35
			};

			double size = sp.Height / 4.0;

			// Parent hexagon
			var parentHex = new HexagonShape(new Hexagon(new TerrainInfo()), size);
			parentHex.Colour = Color.FromArgb(32, 255, 255, 255);
			parentHex.HighlightOnHover = false;
			Canvas.SetLeft(parentHex, (canvas.Width - Hexagon.GetWidth(size)) / 2.0);
			Canvas.SetTop(parentHex, (canvas.Height - Hexagon.GetHeight(size)) / 2.0);
			canvas.Children.Add(parentHex);

			// The child hexagon size calculation
			double nextSize = size * pattern.ChildSizeFactor;

			// Get the width and height for this the next size down of this hexagon
			double hexWidth = Hexagon.GetWidth(nextSize);
			double hexHeight = Hexagon.GetHeight(nextSize);

			// Multiply all the child offsets by the calculated hexagon width / height
			var offsets = pattern.ChildrenInfo
				.Select(x => x.Offset)
				.Select(x => new Point(x.X * hexWidth, x.Y * hexHeight))
				.ToArray();

			// Create each hexagon
			foreach (Point p in offsets) {
				var hex = new HexagonShape(new Hexagon(new TerrainInfo()), nextSize);
				hex.Colour = Color.FromArgb(192, 255, 255, 255);
				hex.HighlightOnHover = false;
				Canvas.SetLeft(hex, (canvas.Width / 2.0) + p.X - (hexWidth / 2.0));
				Canvas.SetTop(hex, (canvas.Height / 2.0) + p.Y - (hexHeight / 2.0));
				canvas.Children.Add(hex);
			}
			sp.Children.Add(canvas);

			// Create name
			var border = new Border() {
				Background = new SolidColorBrush(Colors.DarkGray),
				Child = new TextBlock() {
					Text = pattern.Name,
					FontSize = 20,
					TextAlignment = TextAlignment.Center,
					Height = 35
				}
			};
			sp.Children.Add(border);

			// Return the stack panel container
			return sp;
		}

		#region NavigationHelper registration

		/// <summary>
		/// Invoked when the Page is loaded and becomes the current source of a parent Frame.
		/// </summary>
		/// <param name="e">
		/// Event data that can be examined by overriding code. The event data is representative of the pendingnavigation that
		/// will load the current Page. Usually the most relevant property to examine is Parameter.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navigationHelper.OnNavigatedTo(e);
		}

		/// <summary>
		/// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
		/// </summary>
		/// <param name="e">
		/// Event data that can be examined by overriding code. The event data is representative of the navigation that has
		/// unloaded the current Page.
		/// </param>
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			_navigationHelper.OnNavigatedFrom(e);
		}

		#endregion

	}
}
