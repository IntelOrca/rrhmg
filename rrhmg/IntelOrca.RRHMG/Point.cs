using System;

namespace IntelOrca.RRHMG
{
    /// <summary>
    /// Represents a double precision point defined by X, Y coordinates.
    /// </summary>
    struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
            : this()
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var p = (Point)obj;
            return (p.X == X && p.Y == Y);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return String.Format("X = {0} Y = {1}", X, Y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        public static Point operator /(Point a, Point b)
        {
            return new Point(a.X / b.X, a.Y / b.Y);
        }
    }
}