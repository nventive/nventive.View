using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

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
	/// A converter that is used to change the character casing of a string.
	/// 
	/// CasingType (CasingType) : Provides the type of casing change that is requested.
	/// 
	/// By default, the CasingType is set to UpperCase.
	/// 
	/// This converter may be used when strings that are fetched from a source do not match the
	/// casing that is specified in the designs.
	/// </summary>
    public class FromStringToCasedStringConverter : IValueConverter
    {
		public FromStringToCasedStringConverter()
		{
			this.CasingType = Converters.CasingType.UpperCase;
		}

		public CasingType CasingType { get; set; }

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		public object Convert(object value, Type targetType, object parameter, GenericCulture culture)
		{
			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}

			var text = value as string;

			if (text == null && value != null)
			{
				throw new ArgumentException($"Value must either be null or a string. Got {value} ({value.GetType().FullName})");
			}

			if (string.IsNullOrWhiteSpace(text))
			{
				return value;
			}

#if !WINDOWS_UWP && !__ANDROID__ && !__IOS__ && !__WASM__
			var cultureInfo = culture ?? CultureInfo.CurrentCulture;
			return this.CasingType == Converters.CasingType.LowerCase ?
				text.ToLower(cultureInfo) : 
				text.ToUpper(cultureInfo);
#else
			return this.CasingType == Converters.CasingType.LowerCase ?
				text.ToLower() : 
				text.ToUpper();
#endif
		}

		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture culture)
		{
			throw new NotSupportedException();
		}
	}

	public enum CasingType
	{
 		UpperCase,
		LowerCase
	}
}
