using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IntelOrca.RRHMG.Metro
{
	/// <summary>
	/// Represents a XAML Hexagon shape control.
	/// </summary>
	public sealed class HexagonShape : UserControl
	{
		private readonly Polygon _polygon;

		#region Properties

		public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double),
			typeof(HexagonShape), new PropertyMetadata(32.0, new PropertyChangedCallback((d, e) =>
				((HexagonShape)d).UpdateContent()
			))
		);

		/// <summary>
		/// Gets or sets the size of the hexagon.
		/// </summary>
		public double Size
		{
			get { return (double)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="HexagonShape" /> class.
		/// </summary>
		public HexagonShape()
		{
			Content = _polygon = new Polygon();
			
			this.Loaded += (s, e) => UpdateContent();

			this.AddPropertyChangedHandler<Brush>("BorderBrush", UpdateContent);
			this.AddPropertyChangedHandler<Brush>("Foreground", UpdateContent);
		}

		/// <summary>
		/// Updates the content polygon points and properties.
		/// </summary>
		private void UpdateContent()
		{
			// TODO Move this logic and data to Hexagon core classes.
			double hexagonWidth = Size * 2.0;
			double hexagonHeight = Math.Sqrt(3) / 2.0 * hexagonWidth;
			var offsets = new Point[] {
				new Point(-0.5 + 0.5, 0 + 0.5),
				new Point(-0.25 + 0.5, -0.5 + 0.5),
				new Point(0.25 + 0.5, -0.5 + 0.5),
				new Point(0.5 + 0.5, 0 + 0.5),
				new Point(0.25 + 0.5, 0.5 + 0.5),
				new Point(-0.25 + 0.5, 0.5 + 0.5),
			};

			// Set the polygon points
			_polygon.Points.Clear();
			foreach (Point p in offsets)
				_polygon.Points.Add(new Point(p.X * hexagonWidth, p.Y * hexagonHeight));

			// Set the appearance
			_polygon.Fill = Foreground;
			_polygon.Stroke = BorderBrush;

			// Set the final control size
			Width = hexagonWidth;
			Height = hexagonHeight;
		}
	}
}
