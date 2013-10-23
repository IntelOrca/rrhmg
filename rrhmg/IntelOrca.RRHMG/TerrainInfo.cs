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
			double avg = hexagon.TerrainInfo.Height;
			avg = hexagon.Parent == null ?
				hexagon.TerrainInfo.Height :
				hexagon.Parent.Children.Average(x => x.TerrainInfo.Height);

			ti.Height = avg;
			if (avg < 0.25) {
				if (random.Next(0, 24) == 0)
					ti.Height = 1.0 - avg;
			} else {
				if (random.Next(0, 8) == 0)
					ti.Height = 1.0 - avg;
			}

			return ti;
		}
	}
}
