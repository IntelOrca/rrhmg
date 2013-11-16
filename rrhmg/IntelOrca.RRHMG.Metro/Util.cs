using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
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
	}
}
