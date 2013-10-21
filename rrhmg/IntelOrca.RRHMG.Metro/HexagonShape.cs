using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IntelOrca.RRHMG.Metro
{
	/// <summary>
	/// Represents a XAML Hexagon shape control.
	/// </summary>
	public sealed class HexagonShape : ContentControl
	{
		private readonly Hexagon _hexagon;
		private readonly Polygon _polygon;
		private Color _colour;

		public Hexagon Hexagon { get { return _hexagon; } }

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
		public HexagonShape(Hexagon hexagon, double size)
		{
			_hexagon = hexagon;
			Content = _polygon = new Polygon();
			Size = size;

			UpdateContent();
		}

		/// <summary>
		/// Updates the content polygon points and properties.
		/// </summary>
		private void UpdateContent()
		{
			double hexagonWidth = Hexagon.GetWidth(Size);
			double hexagonHeight = Hexagon.GetHeight(Size);

			// Set the polygon points
			_polygon.Points.Clear();
			foreach (Point offset in Hexagon.PointSizeOffsets.Select(p => new Point(p.X + 0.5, p.Y + 0.5)))
				_polygon.Points.Add(new Point(offset.X * hexagonWidth, offset.Y * hexagonHeight));

			// Set the appearance
			_colour = TerrainRenderer.GetTerrainColour(_hexagon.TerrainInfo);
			_polygon.Fill = new SolidColorBrush(_colour);

			// Set the final control size
			Width = hexagonWidth;
			Height = hexagonHeight;
		}

		protected override void OnPointerEntered(PointerRoutedEventArgs e)
		{
			base.OnPointerEntered(e);

			Color c = _colour;
			c.R = (byte)Math.Min(255, c.R + 128);
			c.G = (byte)Math.Min(255, c.G + 128);
			c.B = (byte)Math.Min(255, c.B + 128);
			_polygon.Fill = new SolidColorBrush(c);
		}

		protected override void OnPointerExited(PointerRoutedEventArgs e)
		{
			base.OnPointerExited(e);

			_polygon.Fill = new SolidColorBrush(_colour);
		}
	}
}
