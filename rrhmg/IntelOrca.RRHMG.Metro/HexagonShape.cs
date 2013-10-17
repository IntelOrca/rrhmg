using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Shapes;

namespace IntelOrca.RRHMG.Metro
{
	public sealed class HexagonShape : UserControl
	{
		private readonly Polygon _polygon;

		public HexagonShape()
		{
			Content = _polygon = new Polygon();

			BindingOperations.SetBinding(_polygon, Polygon.FillProperty, new Binding() { Source = this, ElementName = "Foreground" });

			OnSizeChanged();
		}

		private void OnSizeChanged()
		{
			double hexagonWidth = Size * 2.0;
			double hexagonHeight = Math.Sqrt(3) / 2.0 * hexagonWidth;
			var offsets = new Point[] {
				new Point(-0.5, 0),
				new Point(-0.25, -0.5),
				new Point(0.25, -0.5),
				new Point(0.5, 0),
				new Point(0.25, 0.5),
				new Point(-0.25, 0.5),
			};

			_polygon.Points.Clear();
			foreach (Point p in offsets)
				_polygon.Points.Add(new Point(p.X * hexagonWidth, p.Y * hexagonHeight));
			// _polygon.Fill = Foreground;
			// _polygon.Stroke = BorderBrush;
		}

		public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double),
			typeof(HexagonShape), new PropertyMetadata(64.0, new PropertyChangedCallback((d, e) =>
				((HexagonShape)d).OnSizeChanged()
			))
		);

		public double Size
		{
			get { return (double)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}
	}
}
