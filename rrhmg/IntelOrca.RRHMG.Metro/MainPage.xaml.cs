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
		private int _levels = 0;
		private Point _translate;
		private Point _lastPosition;

		private uint? _movePointer;

        public MainPage()
        {
            this.InitializeComponent();

			this.Loaded += (s, e) => GenerateHexagons();
        }

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

		protected override void OnTapped(TappedRoutedEventArgs e)
		{
			base.OnTapped(e);

			_levels--;
			GenerateHexagons();
		}

		protected override void OnRightTapped(RightTappedRoutedEventArgs e)
		{
			base.OnRightTapped(e);

			_levels++;
			GenerateHexagons();
		}

		private void GenerateHexagons()
		{
			_rand = new Random(0);

			XamlCanvas.RenderTransform = new TranslateTransform() { X = _translate.X, Y = _translate.Y };

			double cx = XamlCanvas.ActualWidth / 2.0;
			double cy = XamlCanvas.ActualHeight / 2.0;
			double size = Math.Min(XamlCanvas.ActualWidth, XamlCanvas.ActualHeight) / 2.0;

			XamlCanvas.Children.Clear();
			// XamlCanvas.Children.Add(GenerateHexagon(size, new Point(cx, cy), 0).First());
			foreach (HexagonShape hex in GenerateHexagon(size, new Point(cx, cy), _levels))
				XamlCanvas.Children.Add(hex);
		}

		private IEnumerable<HexagonShape> GenerateHexagon(double size, Point position, int levels)
		{
			if (levels < 0)
				yield break;

			IEnumerable<Point> offsets = new Point[] {
				new Point(-0.75, -0.5),
				new Point(-0.75, +0.5),
				new Point( 0.00, -1.0),
				new Point( 0.00,  0.0),
				new Point( 0.00, +1.0),
				new Point(+0.75, -0.5),
				new Point(+0.75, +0.5),
			};

			double nextSize = size / 3.0;

			double hexWidth = size / 1.5;
			double hexHeight = Math.Sqrt(3) / 2.0 * hexWidth;
			offsets = offsets.Select(x => new Point(x.X * hexWidth, x.Y * hexHeight));

			if (levels == 0) {
				yield return GenerateHexagon(size, position);
				yield break;
			}

			var hexagons = new List<HexagonShape>();
			foreach (Point p in offsets)
				hexagons.AddRange(GenerateHexagon(nextSize, new Point(position.X + p.X, position.Y + p.Y), levels - 1));

			foreach (HexagonShape hex in hexagons)
				yield return hex;
		}

		private Random _rand = new Random();

		private HexagonShape GenerateHexagon(double size, Point position)
		{
			Color randomColour = Color.FromArgb(
				128,
				(byte)Math.Min(255, _rand.Next(0, 4) * 64),
				(byte)Math.Min(255, _rand.Next(0, 4) * 64),
				(byte)Math.Min(255, _rand.Next(0, 4) * 64)
			);

			var hex = new HexagonShape() {
				Size = size,
				Foreground = new SolidColorBrush(randomColour)
			};

			Canvas.SetLeft(hex, position.X - (hex.Width / 2.0));
			Canvas.SetTop(hex, position.Y - (hex.Height / 2.0));
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
