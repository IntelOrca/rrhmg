#region RRHMG, © Copyright Ted John 2013
// Recursive random hexagon map generator
// Intelorca.RRHMG
// 
// Universiy of Manchester, Computer Science
// Third year project
//
// Application to generate random hexagonal maps of terrain where the map can recurse indefinitely
// in the same manner as a fractal.
// 
// © Copyright Ted John 2013
#endregion

using System;

namespace IntelOrca.RRHMG
{
    /// <summary>
    /// Represents a double precision rectangle defined with X, Y coordinates and a width and height.
    /// </summary>
    struct Rectangle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double x, double y, double width, double height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Contains(Point p)
        {
            return (p.X >= X && p.Y >= Y && p.X < Right && p.Y < Bottom);
        }

        public bool Contains(Rectangle rect)
        {
            return (rect.X >= X && rect.Y >= Y && rect.Right <= Right && rect.Bottom <= Bottom);
        }

        public bool Intersects(Rectangle rect)
        {
            return !(
                rect.X > Right ||
                rect.Right < X ||
                rect.Y > Bottom ||
                rect.Bottom < Y
            );
        }

        public override bool Equals(object obj)
        {
            var rect = (Rectangle)obj;
            return (rect.X == X && rect.Y == Y && rect.Width == Width && rect.Height == Height);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            hash = (hash * 7) + Width.GetHashCode();
            hash = (hash * 7) + Height.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return String.Format("X = {0} Y = {1} Width = {2} Height = {3}", X, Y, Width, Height);
        }

        public Point Location
        {
            get { return new Point(X, Y); }
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
            set {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public double Right
        {
            get { return X + Width; }
        }

        public double Bottom
        {
            get { return Y + Height; }
        }
    }
}