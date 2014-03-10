
using System;
namespace IntelOrca.RRHMG
{
	/// <summary>
	/// Static methods providing useful mathematical functions.
	/// </summary>
	public static class MathX
	{
		/// <summary>
		/// Returns a linear interpolation between two values by the specified ratio.
		/// </summary>
		/// <param name="a">Left value.</param>
		/// <param name="b">Right value.</param>
		/// <param name="t">
		/// The ratio to interpolate between <paramref name="a"/> and <paramref name="b"/>. 0 will return <paramref name="a"/>,
		/// 1 will return <paramref name="b"/>.
		/// </param>
		/// <returns>The linear interpolation between <paramref name="a"/> and <paramref name="b"/>.</returns>
		public static double Lerp(double a, double b, double t)
		{
			return a + t * (b - a);
		}

		/// <summary>
		/// Returns the the specified number or the lower / higher bound if it lies outside the specified range.
		/// </summary>
		/// <param name="x">The number to clamp.</param>
		/// <param name="low">The lower bound.</param>
		/// <param name="high">The upper bound.</param>
		/// <returns><paramref name="low"/>, <paramref name="x"/> or <paramref name="high"/>.</returns>
		public static double Clamp(double x, double low, double high)
		{
			if (x < low)
				return low;
			if (x > high)
				return high;
			return x;
		}

		/// <summary>
		/// Gets a random double value between -1.0 and 1.0.
		/// </summary>
		/// <param name="random">The random.</param>
		/// <returns>A random double between -1.0 and 1.0.</returns>
		public static double NextDoubleSigned(this Random random)
		{
			return (random.NextDouble() * 2.0) - 1.0;
		}
	}
}
