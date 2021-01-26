using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Uno.Extensions;
#if WINDOWS_UWP
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using GenericCulture = System.String;
#elif __ANDROID__ || __IOS__ || __WASM__
using GenericCulture = System.Globalization.CultureInfo;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter is used to provide an easily readable and comprehensible string based on a quantity.
	/// 
	/// Formatter (IPrettyQuantityFormatter) : Provides the converter with a specific way to format the quantity.
	/// 
	/// Formatter has a default implementations which provide expectable and standardized results.
	/// The Formatter should be configured appropriately in XAML. By default, the quantity format is set to one decimal floating point
	/// and there is no separator between the value and the unit label. Actually, only Millions and Thousands are supported.
	/// If the value is null, an empty string will be returned.
	/// 
	/// This converter is used to convert a quantity (double) to a string.
	/// </summary>
	public class FromQuantityToPrettyStringConverter : FromNumberToPrettyStringConverter
	{
		static IPrettyQuantityFormatter DefaultFormatter = new PrettyQuantityFormatter();
		public IPrettyQuantityFormatter Formatter { get; set; }

		public FromQuantityToPrettyStringConverter()
		{
			Formatter = DefaultFormatter;
		}

		protected override string FormatValue(CultureInfo culture, double value)
		{
			return Formatter.Format(culture, value);
		}
	}
}
