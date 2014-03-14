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
		private readonly Polygon _polygon;
		private Color _colour;

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

		/// <summary>
		/// Gets or sets the <see cref="Hexagon"/> this shape is based on.
		/// </summary>
		public Hexagon Hexagon { get; set; }

		/// <summary>
		/// Gets or sets the colour of the hexagon.
		/// </summary>
		public Color Colour
		{
			get { return _colour; }
			set
			{
				_colour = value;
				_polygon.Fill = new SolidColorBrush(_colour);
			}
		}

		/// <summary>
		/// Gets or sets whether the hexagon should highlight when the cursor is over it.
		/// </summary>
		public bool HighlightOnHover { get; set; }

		/// <summary>
		/// Gets whether the hexagon is highlighted or not.
		/// </summary>
		public bool Highlighted { get; private set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="HexagonShape" /> class.
		/// </summary>
		public HexagonShape(Hexagon hexagon, double size)
		{
			Hexagon = hexagon;
			Content = _polygon = new Polygon();
			Size = size;
			HighlightOnHover = true;

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
			DeriveColour();

			// Set the final control size
			Width = hexagonWidth;
			Height = hexagonHeight;
		}

		/// <summary>
		/// Sets the colour based on the hexagon terrain.
		/// </summary>
		public void DeriveColour()
		{
			_colour = TerrainRenderer.GetTerrainColour(Hexagon.TerrainInfo);
			_polygon.Fill = new SolidColorBrush(_colour);
		}

		/// <summary>
		/// Called before the PointerEntered event occurs.
		/// </summary>
		/// <param name="e">Event data for the event.</param>
		protected override void OnPointerEntered(PointerRoutedEventArgs e)
		{
			base.OnPointerEntered(e);

			if (HighlightOnHover) {
				Color c = _colour;
				c.R = (byte)Math.Min(255, c.R + 128);
				c.G = (byte)Math.Min(255, c.G + 128);
				c.B = (byte)Math.Min(255, c.B + 128);
				_polygon.Fill = new SolidColorBrush(c);

				Highlighted = true;
			}
		}

		/// <summary>
		/// Called before the PointerExited event occurs.
		/// </summary>
		/// <param name="e">Event data for the event.</param>
		protected override void OnPointerExited(PointerRoutedEventArgs e)
		{
			base.OnPointerExited(e);

			if (HighlightOnHover) {
				_polygon.Fill = new SolidColorBrush(_colour);
				Highlighted = false;
			}
		}
	}
}
