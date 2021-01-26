using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Uno.Extensions;
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
	/// <para>A converter that applies a <seealso cref="String.Format(GenericCulture, object, object)"/> to a given object.</para>
	/// <para>This format string can either be provided via the <seealso cref="Binding.ConverterParameter"/> or the StringFormat property.</para>
	/// <para>When using numeric values, the supported format is improved compared to the original numeric custom formats. Normally, a format 
	/// group can contain up to three sections, separated by a semi-colon (;). The first section applies to positive values, the second to 
	/// negative values and the third to zero. You can leave the second or third group empty to use the positive format for the
	/// applicable value range. This converter adds support for a fourth and fifth section for values matching 1 or minus 1. Just like the
	/// third group gets matched if the actual formatted value (from the first or second sections) is zero, these new sections will be
	/// used if the formatted value becomes 1 or -1.</para>
	/// <para>This converter may be used when we need to wrap the string representation of an object in a context.
	/// E.G. currencies.</para>
	/// </summary>
	/// <remarks> Never set both the <seealso cref="Binding.ConverterParameter"/> and <seealso cref="StringFormat"/> property.
	/// The converter will throw an <seealso cref="System.ArgumentException"/>. If you set none, it will simply return the
	/// string representation of the value.</remarks>
	public class FromObjectToFormattedStringConverter : IValueConverter
	{
		/// <summary>
		/// <para>The format string to use with the value to build the result string. If empty, 
		/// the <seealso cref="Binding.ConverterParameter"/> is used.</para>
		/// <para>For numeric values, this property supports up to five custom formatting groups. For example, with a en-US culture, 
		/// the format "{0:C;C;broke;a buck}" would display "$1.42" for value 1.42, "broke" for values 0, 0.004 or -0.004, and "a buck" 
		/// for values 1, 0.995 or 1.004.</para>
		/// </summary>
		public string StringFormat { get; set; }

		/// <summary>
		/// Gets or sets the value to return if the incoming value is null. If this property is null, it is ignored and
		/// any null incoming value is still used as an argument.
		/// </summary>
		public string NullValue { get; set; }

		/// <summary>
		/// Allows the consumer of this converter to override the device's UI culture.
		/// </summary>
		public GenericCulture CultureOverride { get; set; }

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		public object Convert(object value, Type targetType, object parameter, GenericCulture culture)
		{
			var format = this.StringFormat;

			if (string.IsNullOrWhiteSpace(format))
			{
				if (parameter == null)
				{
					return value?.ToString() ?? NullValue; // nothing to do
				}

				format = parameter.ToString();
			}
			else if(parameter != null)
			{
				// We are strict: It doesn't make sense to have both formats set.
				throw new ArgumentException($"Both the StringFormat property ({StringFormat}) and ConverterParameter value ({parameter}) are set. Only one can be used at a time.");
			}

			if ((value == null) && (NullValue != null))
			{
				return NullValue;
			}

#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
			culture = CultureOverride.HasValue() ? CultureOverride : culture;
			var currentCulture = string.IsNullOrWhiteSpace(culture) ? CultureInfo.CurrentUICulture : new CultureInfo(culture);
#else
			var currentCulture = CultureOverride ?? culture;
#endif

			return Uno.Extensions.StringExtensions.Format(currentCulture, format, value);
		}


		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture culture)
		{
			throw new NotSupportedException();
		}
	}
}
