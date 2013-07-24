using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntelOrca.RRHMG.Prototype
{
    class MainForm : Form
    {
        private Hexagon _head;
        private double ViewX, ViewY;
        private double _scale = 256.0;
        private int MaxLevels = 0;

        public MainForm()
        {
            BackColor = Color.Black;
            ClientSize = new Size(800, 600);
            DoubleBuffered = true;

            ViewX = 0;
            ViewY = 0;

            Generate();
        }

        private void Recurse(Hexagon h)
        {
            if (h.Level >= MaxLevels)
                return;

            h.RecurseCreate();
            foreach (Hexagon hh in h.Children)
                Recurse(hh);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Middle)
                MaxLevels--;
            if (e.Button == MouseButtons.Right)
                MaxLevels++;
            else
                return;
            Generate();
        }

        private void Generate()
        {
            _head = new Hexagon { X = 0, Y = 0, Size = 1.0, Level = 0 };
            Recurse(_head);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

			g.TranslateTransform(ClientSize.Width / 2.0f, ClientSize.Height / 2.0f);
			g.TranslateTransform(-(float)ViewX, -(float)ViewY);

			RectangleF viewBounds = new RectangleF(
				(float)ViewX - (ClientSize.Width / 2.0f), (float)ViewY - (ClientSize.Height / 2.0f),
				ClientSize.Width, ClientSize.Height
			);

            var queue = new Queue<Hexagon>();
            queue.Enqueue(_head);
            while (queue.Count > 0)
            {
                Hexagon hexagon = queue.Dequeue();

				RectangleF bounds = hexagon.Bounds;
				bounds.X *= (float)_scale;
				bounds.Y *= (float)_scale;
				bounds.Width *= (float)_scale;
				bounds.Height *= (float)_scale;

				if (viewBounds.IntersectsWith(bounds)) {
					if (hexagon.Size / 2.0 * _scale < 10) {
						hexagon.Draw(g, _scale);
					} else {
						if (hexagon.Children.Count == 0)
							hexagon.RecurseCreate();
						foreach (Hexagon h in hexagon.Children)
							queue.Enqueue(h);
					}
				}
            }

			this.Text = String.Format("{0}, {1}", ViewX, ViewY);
        }

        private Point _lastCursor;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left) {
                ViewX -= e.X - _lastCursor.X;
                ViewY -= e.Y - _lastCursor.Y;
                Invalidate();
            }

            _lastCursor = e.Location;    
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta > 0)
            {
                _scale *= 2.0;
				ViewX *= 2.0;
				ViewY *= 2.0;
                Invalidate();
            } else if (e.Delta < 0)
            {
                _scale /= 2.0;
				ViewX /= 2.0;
				ViewY /= 2.0;
                Invalidate();
            }
        }
    }
}
