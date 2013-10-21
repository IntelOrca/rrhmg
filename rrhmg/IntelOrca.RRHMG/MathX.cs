
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
	}
}
