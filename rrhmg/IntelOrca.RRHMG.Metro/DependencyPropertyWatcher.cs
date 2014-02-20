using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IntelOrca.RRHMG.Metro
{
	/// <summary>
	/// Represents an object that handles a dependency binding to provide a value change event.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <remarks>Based on code from http://blogs.msdn.com/b/flaviencharlon/archive/2012/12/07/getting-change-notifications-from-any-dependency-property-in-windows-store-apps.aspx </remarks>
	internal class DependencyPropertyWatcher<T> : DependencyObject, IDisposable
	{
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value",
				typeof(object),
				typeof(DependencyPropertyWatcher<T>),
				new PropertyMetadata(null, OnPropertyChanged));

		public event EventHandler PropertyChanged;

		public DependencyPropertyWatcher(DependencyObject target, string propertyPath)
		{
			this.Target = target;
			BindingOperations.SetBinding(
				this,
				ValueProperty,
				new Binding() { Source = target, Path = new PropertyPath(propertyPath), Mode = BindingMode.OneWay });
		}

		public DependencyObject Target { get; private set; }

		public T Value
		{
			get { return (T)this.GetValue(ValueProperty); }
		}

		public static void OnPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			DependencyPropertyWatcher<T> source = (DependencyPropertyWatcher<T>)sender;

			if (source.PropertyChanged != null) {
				source.PropertyChanged(source, EventArgs.Empty);
			}
		}

		public void Dispose()
		{
			this.ClearValue(ValueProperty);
		}
	}
}
