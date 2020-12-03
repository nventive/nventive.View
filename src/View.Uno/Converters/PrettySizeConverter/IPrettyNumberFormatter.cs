using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Chinook.View.Converters
{
	public interface IPrettyNumberFormatter
	{
		string Format(CultureInfo culture, double value);
		string DefaultFormat { get; set; }
	}
}