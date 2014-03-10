using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace IntelOrca.RRHMG
{
	/// <summary>
	/// Static methods for rendering terrain in some way.
	/// </summary>
	public static class TerrainRenderer
	{
		private static IReadOnlyList<KeyValuePair<double, Color>> ColourMappings = new KeyValuePair<double, Color>[] {
			new KeyValuePair<double, Color>(0.0, Color.FromArgb(255, 0, 0, 64)),
			new KeyValuePair<double, Color>(0.2, Color.FromArgb(255, 0, 0, 255)),
			new KeyValuePair<double, Color>(0.3, Color.FromArgb(255, 64, 64, 0)),
			new KeyValuePair<double, Color>(0.4, Color.FromArgb(255, 128, 128, 0)),
			new KeyValuePair<double, Color>(0.5, Color.FromArgb(255, 0, 64, 0)),
			new KeyValuePair<double, Color>(0.8, Color.FromArgb(255, 0, 255, 0)),
			new KeyValuePair<double, Color>(0.9, Color.FromArgb(255, 64, 64, 64)),
			new KeyValuePair<double, Color>(1.0, Color.FromArgb(255, 255, 255, 255))
		};

		/// <summary>
		/// Gets a solid colour representing the terrain of a map cell.
		/// </summary>
		/// <param name="info">The terrain info.</param>
		/// <returns>A solid colour.</returns>
		public static Color GetTerrainColour(TerrainInfo info)
		{
			//if (!info.Visible)
				//return Colors.Transparent;

			var orderedEntries = ColourMappings.OrderBy(x => x.Key).ToArray();
			KeyValuePair<double, Color> lowerBoundEntry = orderedEntries.LastOrDefault(x => x.Key <= info.Height);
			KeyValuePair<double, Color> upperBoundEntry = orderedEntries.FirstOrDefault(x => x.Key >= info.Height);

			if (lowerBoundEntry.Equals(default(KeyValuePair<double, Color>)))
				lowerBoundEntry = orderedEntries.First();
			if (upperBoundEntry.Equals(default(KeyValuePair<double, Color>)))
				upperBoundEntry = orderedEntries.Last();

			if (lowerBoundEntry.Equals(upperBoundEntry))
				return lowerBoundEntry.Value;

			double t = (info.Height - lowerBoundEntry.Key) / upperBoundEntry.Key;
			byte r = (byte)MathX.Lerp(lowerBoundEntry.Value.R, upperBoundEntry.Value.R, t);
			byte g = (byte)MathX.Lerp(lowerBoundEntry.Value.G, upperBoundEntry.Value.G, t);
			byte b = (byte)MathX.Lerp(lowerBoundEntry.Value.B, upperBoundEntry.Value.B, t);
			return Color.FromArgb(255, r, g, b);

			/*
			if (info.Height < 0.25) {
				// Return a blue colour between 64 and 255 (0.00 - 0.25)
				return Color.FromArgb(255, 0, 0, (byte)MathX.Lerp(64, 255, info.Height / 0.25));
			} else if (info.Height < 4) {
				// Return a yellow colour between 128 and 255 (0.25 - 0.4)
				byte lum = (byte)MathX.Lerp(128, 255, (info.Height - 0.25) / 0.15);
				return Color.FromArgb(255, lum, lum, 0);
			} else if (info.Height > 0.9) {
				// Return a snow colour between 128 and 255 (0.8 - 1.00)
				byte lum = (byte)MathX.Lerp(192, 255, (info.Height - 0.8) / 0.2);
				return Color.FromArgb(255, lum, lum, lum);
			} else {
				// Return a green colour between 64 and 255 (0.25 - 1.00)
				return Color.FromArgb(255, 0, (byte)MathX.Lerp(64, 255, (info.Height - 0.25) / 0.75), 0);
			}
			*/
		}
	}
}
