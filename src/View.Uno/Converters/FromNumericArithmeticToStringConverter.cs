using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter is used to format a numeric in a string after applying an arithmetic operation to it.
	/// 
	/// MultiplyBy (double) : A value by which the given value should be multiplied.
	/// DivideBy (double) : A value by which the given value should be divided.
	/// Add (double) : A value that should be added to the given value.
	/// Substract (double) : A value that should be subtracted to the given value.
	/// Modulo (double) : A value that should be used to apply a modulo to the given value.
	/// FormatString (string) : The Double.ToString format that should be used.
	/// 
	/// For available formats, please refer to : http://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx
	/// 
	/// The precedence of operators is : (((value * MultiplyBy) / DivideBy) + Add - Substract) % Modulo
	/// 
	/// By default, all values have been set to neutral values so that if no values are set, the result will be the
	/// same as the initial value.
	/// By default, the FormatString is a direct string representation of the double value.
	/// 
	/// This converter may be used when the numeric data that is obtained is not directly significant to the user,
	/// E.G. index 0 is the 1st item of a list.
	/// </summary>
	public class FromNumericArithmeticToStringConverter : ConverterBase
	{
		public FromNumericArithmeticToStringConverter()
		{
			MultiplyBy = 1d;
			DivideBy = 1d;
			Add = 0d;
			Substract = 0d;
			Modulo = double.MaxValue;

			FormatString = "F";

		}

		public double MultiplyBy { get; set; }
		public double DivideBy { get; set; }
		public double Add { get; set; }
		public double Substract { get; set; }
		public double Modulo { get; set; }

		public string FormatString { get; set; }

		private bool IsNumeric(object value)
		{
			if (value is Byte ||
				value is SByte ||
				value is UInt16 ||
				value is UInt32 ||
				value is UInt64 ||
				value is Int16 ||
				value is Int32 ||
				value is Int64 ||
				value is Decimal ||
				value is Double ||
				value is Single)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		protected override object Convert(object value, Type targetType, object parameter)
		{
			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}

			if (value != null && !IsNumeric(value))
			{
				throw new ArgumentException($"Value must either be null or of a numeric type. Got {value} ({value.GetType().FullName})");
			}

			var input = System.Convert.ToDouble(value ?? 0d, CultureInfo.InvariantCulture);

			var result = (((input * MultiplyBy) / DivideBy) + Add - Substract) % Modulo;

			return result.ToString(FormatString, CultureInfo.CurrentCulture);
		}
	}
}
