using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Uno;

namespace Nventive.View.Converters
{
	public interface IPrettySizeFormatter : IPrettyNumberFormatter
	{
		[Legacy("NV0118")]
		string FormatSize(double size, CultureInfo culture, string separator, string numberFormat);

		string ValueFormat { get; set; }

		string KiloByteUnitLabel { get; set; }

		string MegaByteUnitLabel { get; set; }

		string GigaByteUnitLabel { get; set; }

		string TeraByteUnitLabel { get; set; }
	}
}