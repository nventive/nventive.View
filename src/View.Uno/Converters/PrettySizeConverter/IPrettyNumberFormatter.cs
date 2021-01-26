using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nventive.View.Converters
{
	public interface IPrettyNumberFormatter
	{
		string Format(CultureInfo culture, double value);
		string DefaultFormat { get; set; }
	}
}