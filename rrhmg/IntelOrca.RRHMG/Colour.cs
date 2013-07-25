namespace IntelOrca.RRHMG
{
    /// <summary>
    /// Represents a colour.
    /// </summary>
    struct Colour
    {
        public double Alpha { get; set; }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }

        public Colour(double red, double green, double blue)
            : this(1.0, red, green, blue)
        {
        }

        public Colour(double alpha, double red, double green, double blue)
            : this()
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
        }

        #region Colour constants
        
        public static readonly Colour Black = new Colour(0.0, 0.0, 0.0);
        public static readonly Colour White = new Colour(1.0, 1.0, 1.0);

        #endregion
    }
}
