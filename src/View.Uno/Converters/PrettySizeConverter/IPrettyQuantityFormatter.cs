using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nventive.View.Converters
{
	public interface IPrettyQuantityFormatter : IPrettyNumberFormatter
	{
		string ValueFormat { get; set; }
		string ThousandUnitLabel { get; set; }
		string MillionUnitLabel { get; set; }
	}
}