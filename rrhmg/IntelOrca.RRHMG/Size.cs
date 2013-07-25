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

namespace IntelOrca.RRHMG
{
    struct Size
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Size(double width, double height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public double Area
        {
            get { return Width * Height; }
        }

        public static Size operator +(Size a, Size b)
        {
            return new Size(a.Width + b.Width, a.Height + b.Height);
        }

        public static Size operator -(Size a, Size b)
        {
            return new Size(a.Width - b.Width, a.Height - b.Height);
        }

        public static Size operator *(Size a, Size b)
        {
            return new Size(a.Width * b.Width, a.Height * b.Height);
        }

        public static Size operator /(Size a, Size b)
        {
            return new Size(a.Width / b.Width, a.Height / b.Height);
        }

        public static Size operator *(Size a, double b)
        {
            return new Size(a.Width * b, a.Height * b);
        }

        public static Size operator /(Size a, double b)
        {
            return new Size(a.Width / b, a.Height / b);
        }
    }
}