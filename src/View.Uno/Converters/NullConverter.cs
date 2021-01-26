using System;
using System.Globalization;
using Uno.Extensions;
using Uno.Logging;
#if WINDOWS_UWP || __WASM__
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using GenericCulture = System.String;
#elif __ANDROID__
using Android.Views;
using GenericCulture = System.String;
using Windows.UI.Xaml.Data;
#elif __IOS__
using UIKit;
using GenericCulture = System.String;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// A converter which always return null.
	/// </summary>
	internal class NullConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, GenericCulture language)
		{
			this.Log().Warn("Convert a value using the NullConverter (Usually you get this when you specify a converter on a binding, and it does not implemenets IValueConverter).");

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture language)
		{
			this.Log().Warn("Convert BACK a value using the NullConverter (Usually you get this when you specify a converter on a binding, and it does not implemenets IValueConverter).");

			return null;
		}
	}
}
