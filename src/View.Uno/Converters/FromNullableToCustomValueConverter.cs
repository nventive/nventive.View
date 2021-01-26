using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Nventive.View.Converters
{
	/// <summary>
	/// A converter that is used to return a custom value based on the presence or absence of value.
	/// 
	/// ValueIfNull (object) : The custom value that is returned if the value is null.
	/// ValueIfNotNull (object) : The custom value that is returned if the value is not null.
	/// 
	/// If ValueIfNotNull is not set, the converter will return the value if it is not null.
	/// If ValueIfNull is not set, the converter will return the custom value of the type if the value is null.
	/// 
	/// This converter may be used to display a custom value whenever the data is not ready.
	/// </summary>
	public class FromNullableToCustomValueConverter : ConverterBase
	{
		public FromNullableToCustomValueConverter()
		{
			ValueIfNull = null;
			ValueIfNotNull = null;
		}

		public object ValueIfNull { get; set; }

		public object ValueIfNotNull { get; set; }

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		protected override object Convert(object value, Type targetType, object parameter)
		{
			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}

			if (value == null)
			{
				return ValueIfNull ?? GetCustomValue(targetType);
			}
			else
			{
				return ValueIfNotNull ?? value;
			}
		}

		private static object GetCustomValue(Type targetType)
		{
#if SILVERLIGHT
			return targetType.IsValueType ?
#else
			return targetType.GetTypeInfo().IsValueType ? 
#endif
				Activator.CreateInstance(targetType) : 
				null;
		}
	}
}
