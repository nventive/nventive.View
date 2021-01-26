using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Uno.Extensions;
using Uno.Conversion;
#if WINDOWS_UWP
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using GenericCulture = System.String;
#elif __ANDROID__ || __IOS__ || __WASM__
using GenericCulture = System.String;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter uses the conversion extensions that are registered in an application in order
	/// to provide a value, using the targetType.
	/// 
	/// This may be used in order to apply a default conversion from one data type to another inside of an app.
	/// </summary>
    public class ConversionExtensionConverter : IValueConverter
    {
        private IConversionExtensions _extensions;

        public ConversionExtensionConverter(IConversionExtensions extensions)
        {
            _extensions = extensions.Validation().NotNull("extensions");
        }

		public object Convert(object value, Type targetType, object parameter, GenericCulture culture)
		{
			if (parameter != null)
			{
				throw new ArgumentException("This converter does not use any parameters");
			}

			if (value == null)
			{
				return null;
			}

#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
			if (string.IsNullOrWhiteSpace(culture))
			{
				return _extensions.To(_extensions.Conversion(value), targetType);
			}
			else
			{
				return _extensions.To(_extensions.Conversion(value), targetType, new CultureInfo(culture));
			}
#else
            return _extensions.To(_extensions.Conversion(value), targetType, culture);
#endif
		}

		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture culture)
		{
			// The system is the same in both directions
			return Convert(value, targetType, parameter, culture);
		}
	}
}
