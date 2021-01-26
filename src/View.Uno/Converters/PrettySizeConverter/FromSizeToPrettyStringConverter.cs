using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Uno.Extensions;
using Uno;

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter is used to provide an easily readable and comprehensible string based on a file size.
	/// 
	/// Formatter (IPrettySizeFormatter) : Provides the converter with a specific way to format the size.
	/// NumberFormat (string) : Provides the converter with a specific way to format the number.
	/// Separator (string) : Sets the separator string between the number and the units.
	/// 
	/// Formatter has a default implementations which provide expectable and standardized results.
	/// The Formatter should be configured appropriately in XAML. By default, the size format is set to two decimal floating point
	/// and the separator between the size and the unit is a single whitespace.
	/// If the value is null, an empty string will be returned.
	/// 
	/// This converter is used to convert a file size (double) to a string.
	/// </summary>
	public class FromSizeToPrettyStringConverter : FromNumberToPrettyStringConverter
	{
		static IPrettySizeFormatter DefaultFormatter = new PrettySizeFormatter();
		/// <summary>
		/// This boolean allows to track whether if the NumberFormat or Separator obsolete properties have been modified.
		/// </summary>
		private bool _isOldFormattingUsed = false;

		public IPrettySizeFormatter Formatter { get; set; }

		private string _numberFormat = "0:N2";
		/// <summary>
		/// Formatting used in the string format for the number part of the string
		/// </summary>
		[Legacy("NV0118")]
		public string NumberFormat
		{
			get { return _numberFormat; }
			set
			{
				_numberFormat = value;
				_isOldFormattingUsed = true;
			}
		}

		private string _separator = " ";
		/// <summary>
		/// Separator between the number and the unit
		/// </summary>
		[Legacy("NV0118")]
		public string Separator
		{
			get { return _separator; }
			set
			{
				_separator = value;
				_isOldFormattingUsed = true;
			}
		}

		public FromSizeToPrettyStringConverter()
		{
			Formatter = DefaultFormatter;
		}

		protected override string FormatValue(CultureInfo culture, double value)
		{
			if (_isOldFormattingUsed)
			{
				return Formatter.FormatSize(value, culture, Separator, NumberFormat);
			}
			return Formatter.Format(culture, value);
		}
	}
}
