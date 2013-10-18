using System;
using Windows.UI.Xaml;

namespace IntelOrca.RRHMG.Metro
{
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
	}
}
