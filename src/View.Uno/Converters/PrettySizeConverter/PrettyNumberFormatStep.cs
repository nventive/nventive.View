using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	public class PrettyNumberFormatStep
	{
		public double StepValue { get; set; }
		public Func<string> GetFormat { get; set; }
		public Func<string> GetUnitLabel { get; set; }
	}
}
