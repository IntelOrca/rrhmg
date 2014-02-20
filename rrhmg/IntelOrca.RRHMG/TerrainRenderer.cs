using Windows.UI;

namespace IntelOrca.RRHMG
{
	/// <summary>
	/// Static methods for rendering terrain in some way.
	/// </summary>
	public static class TerrainRenderer
	{
		/// <summary>
		/// Gets a solid colour representing the terrain of a map cell.
		/// </summary>
		/// <param name="info">The terrain info.</param>
		/// <returns>A solid colour.</returns>
		public static Color GetTerrainColour(TerrainInfo info)
		{
			if (info.Height < 0.25) {
				// Return a blue colour between 64 and 255 (0.00 - 0.25)
				return Color.FromArgb(255, 0, 0, (byte)MathX.Lerp(64, 255, info.Height / 0.25));
			} else {
				// Return a green colour between 64 and 255 (0.25 - 1.00)
				return Color.FromArgb(255, 0, (byte)MathX.Lerp(64, 255, (info.Height - 0.25) / 0.75), 0);
			}
		}
	}
}
