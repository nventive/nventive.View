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
#else
using System.Windows.Data;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// This is the base class for converters used for formatting double values to string.
	/// If the value is null, an empty string will be returned.
	/// </summary>
	public abstract class FromNumberToPrettyStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, GenericCulture culture)
		{
			if (parameter != null)
			{
				throw new ArgumentException("This converter does not use any parameters");
			}

			double number;

			if (value == null)
			{
				return string.Empty;
			}

#if !WINDOWS_UWP && !__ANDROID__ && !__IOS__ && !__WASM__
			var cultureInfo = culture ?? CultureInfo.CurrentCulture;
#else
			var cultureInfo = culture != null ? new CultureInfo(culture) : CultureInfo.CurrentCulture;
#endif

			if (value is double)
			{
				number = (double)value;
			}
			else
			{
				var parseSuccessful = Double.TryParse(value.ToString(), NumberStyles.None, cultureInfo, out number);

				if (!parseSuccessful)
				{
					throw new ArgumentException("Value must either be null or of type double");
				}
			}

			return FormatValue(cultureInfo, number);
		}

		protected abstract string FormatValue(CultureInfo culture, double value);

		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture culture)
		{
			throw new NotSupportedException();
		}
	}
}
