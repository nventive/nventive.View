using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Uno.Extensions;
#if WINDOWS_UWP || __WASM__
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using GenericCulture = System.String;
#elif __ANDROID__ || __IOS__
using GenericCulture = System.String;
using Windows.UI.Xaml.Data;
#elif HAS_WINUI
using Microsoft.UI.Xaml.Data;
using GenericCulture = System.String;
#else
using System.Windows.Data;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Chinook.View.Converters
{
	/// <summary>
	/// This converter is used to provide an easily readable and comprehensible string based on a distance.
	/// 
	/// DistanceUnit (DistanceUnit?) : Provides the converter with a unit type to use (imperial or metric).
	/// 
	/// If DistanceUnit is not set, the converter will use the current culture's default unit.
	/// 
	/// This converter is used to convert a distance (double) to a string, E.G. to display how far an item is.
	/// </summary>
	public class FromDistanceToPrettyStringConverter : IValueConverter
	{
		public static IPrettyDistanceFormatter DefaultFormatter = new PrettyDistanceFormatter();

		public DistanceUnit DistanceUnit { get; set; }

		public IPrettyDistanceFormatter Formatter { get; set; }

		public FromDistanceToPrettyStringConverter()
		{
			Formatter = DefaultFormatter;
		}

		public object Convert(object value, Type targetType, object parameter, GenericCulture culture)
		{
			if (value == null)
			{
				return null; // nothing to do with a null input parameter
			}

			if (parameter != null)
			{
				throw new ArgumentException("This converter does not use any parameters");
			}

#if !WINDOWS_UWP && !__ANDROID__ && !__IOS__ && !__WASM__
			var cultureInfo = culture ?? CultureInfo.CurrentCulture;
#else
			var cultureInfo = string.IsNullOrWhiteSpace(culture) ?  CultureInfo.CurrentCulture : new CultureInfo(culture);
#endif

			var distance = value.Conversion().To<double>(cultureInfo);

			if (double.IsNaN(distance))
			{
				return null;
			}

			return Formatter.Format(distance, cultureInfo: cultureInfo, distanceOutType: this.DistanceUnit);
		}

		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture culture)
		{
			throw new NotSupportedException();
		}
	}
}
