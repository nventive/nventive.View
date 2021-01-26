using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

#if WINDOWS_UWP
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter is used to get a custom value if a nullable boolean is true or otherwise.
	/// 
	/// NullOrFalseValue (object) : The custom value to return if the value is null or false.
	/// TrueValue (object) : The custom value to return if the value is true.
	/// 
	/// NullOrFalseValue and TrueValue need to be different in order for reverse conversion to work.
	/// 
	/// This converter may be used to select betweem two values based on a boolean value.
	/// </summary>
	public class FromNullableBoolToCustomValueConverter : ConverterBase
	{
		public object NullOrFalseValue { get; set; }
		public object TrueValue { get; set; }

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		protected override object Convert(object value, Type targetType, object parameter)
		{
			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}

			if (value != null && !(value is bool))
			{
				throw new ArgumentException($"Value must either be null or of type bool. Got {value} ({value.GetType().FullName})");
			}

			if (value == null || !System.Convert.ToBoolean(value, CultureInfo.InvariantCulture))
			{
				return NullOrFalseValue;
			}
			else
			{
				return TrueValue;
			}
		}

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		protected override object ConvertBack(object value, Type targetType, object parameter)
		{
			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}

			if (object.Equals(this.TrueValue, this.NullOrFalseValue))
			{
				throw new InvalidOperationException("Cannot convert back if both custom values are the same");
			}

			return this.TrueValue != null ?
				value.Equals(TrueValue) :
				!value.Equals(this.NullOrFalseValue);
		}
	}
}
