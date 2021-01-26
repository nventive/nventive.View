using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Nventive.View.Converters
{
	public enum RoundingType
	{
		Floor,
		Ceiling,
		Round
	}

	public abstract class PrettyNumberFormatterBase : IPrettyNumberFormatter
	{
		protected PrettyNumberFormatterBase() { }

		public string DefaultFormat { get; set; } = "{0}";

		/// <summary>
		/// Gets or sets the number of decimals used for the rounding action. By default the value is -1,
		/// meaning that no decimals will be rounded. To activate the rounding defined in the RoundingAction
		/// property, set the DecimalsCount to 0 or greater.
		/// </summary>
		public int DecimalsCount { get; set; } = -1;

		public RoundingType RoundingAction { get; set; } = RoundingType.Round;

		protected virtual bool IncludeStepValues { get; } = true;

		protected List<PrettyNumberFormatStep> FormattingSteps { get; } = new List<PrettyNumberFormatStep>();

		public virtual string Format(CultureInfo culture, double value)
		{
			var matchingStep = FormattingSteps.FirstOrDefault(step => IncludeStepValues ? value >= step.StepValue : value > step.StepValue);

			if (matchingStep == null)
			{
				return string.Format(culture, DefaultFormat, value);
			}

			var dividedValue = value / matchingStep.StepValue;
			if (DecimalsCount >= 0)
			{
				switch (RoundingAction)
				{
					case RoundingType.Floor:
						dividedValue = Round(dividedValue, DecimalsCount, Math.Truncate);
						break;
					case RoundingType.Ceiling:
						dividedValue = Round(dividedValue, DecimalsCount, Math.Ceiling);
						break;
					default:
						dividedValue = Round(dividedValue, DecimalsCount, Math.Round);
						break;
				}
			}

			return string.Format(culture, matchingStep.GetFormat(), dividedValue, matchingStep.GetUnitLabel());
		}

		private double Round(double value, int nbDecimals, Func<double, double> mathematicalOperation)
		{
			var adjustment = Math.Pow(10, nbDecimals);
			return mathematicalOperation(value * adjustment) / adjustment;
		}
	}
}
