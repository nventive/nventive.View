#if WINDOWS_UWP
// TODO: Remove above if __ANDROID__ || __IOS__ implemented.
using System;
using System.Collections.Generic;
using System.Text;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#elif __ANDROID__ || __IOS__
// TODO
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter allows mapping an <seealso cref="Orientation"/> to any custom value.
	/// </summary>
	/// <remarks>Because an Orientation rarely (and shouldn't) comes from a view-model, this
	/// converter doesn't support null values. If null comes in, null comes out.</remarks>
	public class FromOrientationToDefaultValueConverter : ConverterBase
	{
		public object HorizontalValue { get; set; }
		public object VerticalValue { get; set; }

		protected override object Convert(object value, Type targetType, object parameter)
		{
			if(value is Orientation)
			{
				return ((Orientation)value) == Orientation.Horizontal
					? this.HorizontalValue
					: this.VerticalValue;
			}

			return null;
		}
	}
}
#endif
