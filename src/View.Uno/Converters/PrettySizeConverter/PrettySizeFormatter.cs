using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Uno;

namespace Nventive.View.Converters
{
	public class PrettySizeFormatter : PrettyNumberFormatterBase, IPrettySizeFormatter
	{
		private const double KiloStepUnit = 1024D;
		private const double MegaStepUnit = KiloStepUnit * KiloStepUnit;
		private const double GigaStepUnit = MegaStepUnit * KiloStepUnit;
		private const double TeraStepUnit = GigaStepUnit * KiloStepUnit;

		public PrettySizeFormatter()
		{
			DefaultFormat = "{0:N0} B";

			FormattingSteps.Add(new PrettyNumberFormatStep
			{
				StepValue = TeraStepUnit,
				GetUnitLabel = () => TeraByteUnitLabel,
				GetFormat = () => ValueFormat
			});
			FormattingSteps.Add(new PrettyNumberFormatStep
			{
				StepValue = GigaStepUnit,
				GetUnitLabel = () => GigaByteUnitLabel,
				GetFormat = () => ValueFormat
			});
			FormattingSteps.Add(new PrettyNumberFormatStep
			{
				StepValue = MegaStepUnit,
				GetUnitLabel = () => MegaByteUnitLabel,
				GetFormat = () => ValueFormat
			});
			FormattingSteps.Add(new PrettyNumberFormatStep
			{
				StepValue = KiloStepUnit,
				GetUnitLabel = () => KiloByteUnitLabel,
				GetFormat = () => ValueFormat
			});
		}

		protected override bool IncludeStepValues { get { return false; } }

		public string KiloByteUnitLabel { get; set; } = "kB";

		public string MegaByteUnitLabel { get; set; } = "MB";

		public string GigaByteUnitLabel { get; set; } = "GB";

		public string TeraByteUnitLabel { get; set; } = "TB";

		public string ValueFormat { get; set; } = "{0:N2} {1}";

		[Legacy("NV0118")]
		public string FormatSize(double size, CultureInfo culture, string separator, string numberFormat)
		{
			// This code is used for compatibility with existing classes
			DefaultFormat = "{0:N0}" + separator + "B";
			ValueFormat = "{" + numberFormat + "}" + separator + "{1}";
			return base.Format(culture, size);
		}
	}
}