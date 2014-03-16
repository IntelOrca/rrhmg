using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelOrca.RRHMG
{
	/// <summary>
	/// Represents the terrain for a single map cell.
	/// </summary>
	public class TerrainInfo
	{
		/// <summary>
		/// Gets or sets the height of the terrain between 0 and 1.
		/// </summary>
		public double Height { get; set; }

		public bool Visible { get; set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="TerrainInfo"/> class.
		/// </summary>
		public TerrainInfo()
		{
			Visible = true;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Height.ToString();
		}

		/// <summary>
		/// Creates a new <see cref="TerrainInfo"/> based on the given hexagon.
		/// </summary>
		/// <param name="hexagon">The base hexagon.</param>
		/// <param name="random">The random number generator.</param>
		/// <returns>A new <see cref="TerrainInfo"/>.</returns>
		public static TerrainInfo FromHexagon(Hexagon hexagon, Random random)
		{
			var ti = new TerrainInfo();

			// Basic height variation

			double range = hexagon.Parent == null ?
				0.0 :
				hexagon.Siblings.Max(x => x.TerrainInfo.Height) - hexagon.Siblings.Min(x => x.TerrainInfo.Height);

			double avg = hexagon.TerrainInfo.Height;
			avg = hexagon.Parent == null ?
				hexagon.TerrainInfo.Height :
				hexagon.Siblings.Average(x => x.TerrainInfo.Height);

			ti.Height = avg;

			if (range == 0) {
				// Larger chance of rough
				if (random.Next(0, 2) == 0)
					ti.Height += random.NextDoubleSigned() * 2.0;
			} else {
				// Larger chance of smooth
				if (random.Next(0, 5) == 0) {
					if (avg < 0.25) {
						if (random.Next(0, 4) == 0)
							ti.Height = 1.0 - avg;
					} else {
						if (random.Next(0, 8) == 0)
							ti.Height = 1.0 - avg;
					}
				} else
					ti.Height = MathX.Lerp(hexagon.TerrainInfo.Height, avg, random.NextDouble() * 0.1);
			}

			// Fix height between 0 and 1
			ti.Height = MathX.Clamp(ti.Height, 0.0, 1.0);

			return ti;
		}
	}
}
