using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nventive.View.Converters
{
	public class PrettyQuantityFormatter : PrettyNumberFormatterBase, IPrettyQuantityFormatter
	{
		private const double StepUnit = 1000D;

		public PrettyQuantityFormatter()
		{
			DecimalsCount = 1;
			RoundingAction = RoundingType.Floor;

			FormattingSteps.Add(new PrettyNumberFormatStep
			{
				StepValue = StepUnit * StepUnit,
				GetUnitLabel = () => MillionUnitLabel,
				GetFormat = () => ValueFormat
			});
			FormattingSteps.Add(new PrettyNumberFormatStep
			{
				StepValue = StepUnit,
				GetUnitLabel = () => ThousandUnitLabel,
				GetFormat = () => ValueFormat
			});
		}

		public string ValueFormat { get; set; } = "{0:N1} {1}";

		public string ThousandUnitLabel { get; set; } = "K";

		public string MillionUnitLabel { get; set; } = "M";
	}
}