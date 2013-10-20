using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IntelOrca.RRHMG.Metro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class MainPage : Page
    {
		private int _maxLevelsToShow = 4;
		private Point _translate;
		private Point _lastPosition;

		private uint? _movePointer;

		private Hexagon _headHexagon = new Hexagon(Colors.Blue);
		private Hexagon _showingHexagon;

        public MainPage()
        {
			_headHexagon.Children = new[] {
				new Hexagon(_headHexagon, Colors.Blue),
				new Hexagon(_headHexagon, Colors.Blue),
				new Hexagon(_headHexagon, Colors.Red),
				new Hexagon(_headHexagon, Colors.Red),
				new Hexagon(_headHexagon, Colors.Blue),
				new Hexagon(_headHexagon, Colors.Yellow),
				new Hexagon(_headHexagon, Colors.Yellow),
			};
			_showingHexagon = _headHexagon;

            this.InitializeComponent();

			this.Loaded += (s, e) => GenerateHexagons();
        }

		/*
		protected override void OnPointerPressed(PointerRoutedEventArgs e)
		{
			base.OnPointerPressed(e);

			if (!_movePointer.HasValue) {
				_movePointer = e.Pointer.PointerId;
				_lastPosition = e.GetCurrentPoint(this).Position;
			}
		}

		protected override void OnPointerReleased(PointerRoutedEventArgs e)
		{
			base.OnPointerReleased(e);

			if (_movePointer == e.Pointer.PointerId)
				_movePointer = null;
		}

		protected override void OnPointerMoved(PointerRoutedEventArgs e)
		{
			base.OnPointerMoved(e);

			if (e.Pointer.PointerId != _movePointer)
				return;

			PointerPoint pp = e.GetCurrentPoint(this);
			Point currentPosition = pp.Position;
			if (pp.IsInContact) {
				double dx = currentPosition.X - _lastPosition.X;
				double dy = currentPosition.Y - _lastPosition.Y;

				_translate = new Point(_translate.X + dx, _translate.Y + dy);
				GenerateHexagons();
			}

			ttt.Text = String.Format("{0:000.0}, {1:000.0}", currentPosition.X, currentPosition.Y);
			_lastPosition = currentPosition;			
		}
		*/

		private void GenerateHexagons()
		{
			XamlCanvas.RenderTransform = new TranslateTransform() { X = _translate.X, Y = _translate.Y };

			double cx = XamlCanvas.ActualWidth / 2.0;
			double cy = XamlCanvas.ActualHeight / 2.0;
			double size = Math.Max(XamlCanvas.ActualWidth, XamlCanvas.ActualHeight) * 0.75;

			XamlCanvas.Children.Clear();
			foreach (HexagonShape hex in GenerateHexagon(size, new Point(cx, cy), _maxLevelsToShow, _showingHexagon))
				XamlCanvas.Children.Add(hex);
		}

		private IEnumerable<HexagonShape> GenerateHexagon(double size, Point position, int levels, Hexagon hexagon)
		{
			if (levels < 0)
				yield break;

			IReadOnlyList<Point> offsets = new Point[] {
				new Point(-0.75, -0.5),
				new Point(-0.75, +0.5),
				new Point( 0.00, -1.0),
				new Point( 0.00,  0.0),
				new Point( 0.00, +1.0),
				new Point(+0.75, -0.5),
				new Point(+0.75, +0.5),
			};

			double nextSize = size / 3.0;

			double hexWidth = Hexagon.GetWidth(nextSize);
			double hexHeight = Hexagon.GetHeight(nextSize);
			offsets = offsets.Select(x => new Point(x.X * hexWidth, x.Y * hexHeight)).ToArray();

			if (levels == 0 || hexagon.Children == null) {
				yield return GenerateHexagon(size, position, hexagon);
				yield break;
			}

			var hexagons = new List<HexagonShape>();
			for (int i = 0; i < hexagon.Children.Length; i++) {
				Point p = offsets[i];
				hexagons.AddRange(GenerateHexagon(nextSize, new Point(position.X + p.X, position.Y + p.Y), levels - 1, hexagon.Children[i]));
			}
 
			foreach (HexagonShape hex in hexagons)
				yield return hex;
		}

		private Random _rand = new Random();

		private Color GetRandomColour()
		{
			return Color.FromArgb(
				255,
				(byte)Math.Min(255, _rand.Next(0, 4) * 32),
				(byte)Math.Min(255, _rand.Next(0, 4) * 32),
				(byte)Math.Min(255, _rand.Next(0, 4) * 32)
			);
		}

		private HexagonShape GenerateHexagon(double size, Point position, Hexagon hexagon)
		{
			var hex = new HexagonShape(hexagon, size);

			Canvas.SetLeft(hex, position.X - (hex.Width / 2.0));
			Canvas.SetTop(hex, position.Y - (hex.Height / 2.0));

			hex.Tapped += (s, e) => {
				var shex = s as HexagonShape;
				// _showingHexagon = shex.Hexagon;
				var tappedhex = shex.Hexagon;

				int levelsDown = 1;
				var currenthex = tappedhex;
				while (currenthex.Parent != _showingHexagon) {
					currenthex = currenthex.Parent;
					levelsDown++;
				}

				if (levelsDown < _maxLevelsToShow) {
					if (tappedhex.Children == null) {
						tappedhex.Children =
							Enumerable.Range(0, 7).
							Select(x => new Hexagon(tappedhex, GetRandomColour())).
							ToArray();
					}
				} else {
					_showingHexagon = currenthex;
				}

				GenerateHexagons();
			};

			hex.IsRightTapEnabled = true;
			hex.RightTapped += (s, e) => {
				var shex = s as HexagonShape;
				if (_showingHexagon.Parent != null) {
					_showingHexagon = _showingHexagon.Parent;
					GenerateHexagons();
				}
			};

			return hex;
		}

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
