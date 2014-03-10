using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace IntelOrca.RRHMG.Metro
{
	/// <summary>
	/// Static class containing helper and utility methods.
	/// </summary>
	internal static class Util
	{
		/// <summary>
		/// Allows a property change event to be attached to any dependency property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parent">The parent class of the dependency property.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="a">The action handler.</param>
		public static void AddPropertyChangedHandler<T>(this DependencyObject parent, string propertyName, Action a)
		{
			new DependencyPropertyWatcher<T>(parent, propertyName).PropertyChanged += (s, e) => a();
		}

		/// <summary>
		/// Gets the item at the specified index.
		/// </summary>
		/// <typeparam name="T">The enumerable type.</typeparam>
		/// <param name="items">The enumerable of items.</param>
		/// <param name="index">The index.</param>
		/// <returns>The item at the specified index.</returns>
		public static T Get<T>(this IEnumerable<T> items, int index)
		{
			return items.Skip(index).First();
		}

		/// <summary>
		/// Determines whether this rectangle intersects with the specified rectangle.
		/// </summary>
		/// <param name="a">The rectangle.</param>
		/// <param name="b">The other rectangle.</param>
		/// <returns>True if the rectangles intersect, otherwise false.</returns>
		public static bool IntersectsWith(this Rect a, Rect b)
		{
			return
				(a.Right >= b.Left && a.Left < b.Right) &&
				(a.Bottom >= b.Top && a.Top < b.Bottom);
		}

		/// <summary>
		/// Creates a new WAV stream representing a beep sound given a amplitude, frequency and duration.
		/// </summary>
		/// <param name="amplitude">The amplitude between 0 and 32767.</param>
		/// <param name="frequency">The frequency in hertz.</param>
		/// <param name="duration">The duration in milliseconds.</param>
		/// <returns>A random accessable WAV PCM data stream.</returns>
		/// <remarks>Adapted from
		/// http://social.msdn.microsoft.com/Forums/vstudio/en-US/18fe83f0-5658-4bcf-bafc-2e02e187eb80/beep-beep
		/// </remarks>
		public static async Task<IRandomAccessStream> GetBeepStream(int amplitude, int frequency, int duration)
		{
			// Calculate wave
			double a = ((amplitude * (Math.Pow(2, 15))) / 1000) - 1;
			double deltaFT = 2 * Math.PI * frequency / 44100.0;

			// Prepare WAV header
			int samples = 441 * duration / 10;
			int bytes = samples * 4;
			int[] header = {
				0X46464952, 36 + bytes, 0X45564157, 0X20746D66, 16, 0X20001, 44100, 176400, 0X100004, 0X61746164, bytes
			};

			// Prepare a WAV data stream
			var ims = new InMemoryRandomAccessStream();
			IOutputStream outStream = ims.GetOutputStreamAt(0);
			var dw = new DataWriter(outStream);
			dw.ByteOrder = ByteOrder.LittleEndian;

			// Write header
			for (int i = 0; i < header.Length; i++)
				dw.WriteInt32(header[i]);

			// Write samples
			for (int t = 0; t < samples; t++) {
				short sampleValue = Convert.ToInt16(a * Math.Sin(deltaFT * t));
				dw.WriteInt16(sampleValue);
				dw.WriteInt16(sampleValue);
			}

			// Flush the WAV stream
			await dw.StoreAsync();
			await outStream.FlushAsync();
			return ims;
		}
	}
}
