using System;
using System.Collections.Generic;
using System.Drawing;

namespace IntelOrca.RRHMG.Prototype
{
    class Hexagon
    {
        private static Random rand = new Random();
        private static int NextGIndex = 0;

        private int GIndex;
        public int Level;
        public double X, Y;
        public double Size;
        public Color Colour = Color.White;

        public List<Hexagon> Children = new List<Hexagon>();
        
        public Hexagon()
        {
            GIndex = NextGIndex++;
        }

        public void RecurseCreate()
        {
            Color[] colours = new Color[]
                            {Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Pink, Color.Brown, Color.LightBlue,
                             Color.Teal, Color.Lime, Color.Purple, Color.Silver, Color.Gold, Color.Orange, Color.Navy};

            var hexagon = new Hexagon();
            hexagon.X = X - (Size * 0.25);
            hexagon.Y = Y + Height * -0.5;
            hexagon.Size = Size/2.0;
            hexagon.Level = Level + 1;
            hexagon.Colour = colours[rand.Next(colours.Length)];
            Children.Add(hexagon);

            hexagon = new Hexagon();
            hexagon.X = X - (Size * 0.25);
            hexagon.Y = Y + Height * 0.5;
            hexagon.Size = Size / 2.0;
            hexagon.Level = Level + 1;
            hexagon.Colour = colours[rand.Next(colours.Length)];
            Children.Add(hexagon);

            hexagon = new Hexagon();
            hexagon.X = X + (Size * 0.5);
            hexagon.Y = Y;
            hexagon.Size = Size / 2.0;
            hexagon.Level = Level + 1;
            hexagon.Colour = colours[rand.Next(colours.Length)];
            Children.Add(hexagon);

            hexagon = new Hexagon();
            hexagon.X = X - Size;
            hexagon.Y = Y;
            hexagon.Size = Size / 2.0;
            hexagon.Level = Level + 1;
            hexagon.Colour = colours[rand.Next(colours.Length)];
            Children.Add(hexagon);
        }

        public void Draw(Graphics g, double scale)
        {
            double hexSize = scale*Size;

            var corners = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = 2 * Math.PI / 6.0 * i;
                corners[i] = new PointF(
                    (float)((X * scale) + hexSize * Math.Cos(angle)),
                    (float)((Y * scale) + hexSize * Math.Sin(angle))
                );
            }

            g.FillPolygon(new SolidBrush(Colour), corners);
            // g.DrawString(GIndex.ToString(), new Font(FontFamily.GenericSansSerif, 10.0f), Brushes.White, (float) (X * scale), (float) (Y * scale));
        }

        public double Width { get { return Size; } }
        public double Height { get { return Math.Sqrt(3.0) / 2.0 * Width; } }
    }
}