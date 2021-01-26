using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	/// <summary>
	/// Allows to match a value within a provided interval.
	/// Inclusive and exclusive modes can be set for each bound.
	/// </summary>
	/// <remarks>
	/// If <see cref="LowerBound" /> is not provided, <see cref="double.MinValue" /> will be used.
	/// If <see cref="UpperBound" /> is not provided, <see cref="double.MaxValue" /> will be used.
	/// If <see cref="LowerBoundMode" /> is not provided, <see cref="BoundMode.Inclusive" /> will be used.
	/// If <see cref="UpperBoundMode" /> is not provided, <see cref="BoundMode.Inclusive" /> will be used.
	/// </remarks>
	public class IntervalPartition : Partition
	{
		/// <summary>
		/// The lower bound for a conversion value to be considered in range.
		/// </summary>
		public double LowerBound { get; set; } = double.MinValue;

		/// <summary>
		/// The max value for a conversion value to be considered in range.
		/// </summary>
		public double UpperBound { get; set; } = double.MaxValue;

		/// <summary>
		/// Lower bound mode.
		/// </summary>
		public BoundMode LowerBoundMode { get; set; } = BoundMode.Inclusive;

		/// <summary>
		/// Upper bound mode.
		/// </summary>
		public BoundMode UpperBoundMode { get; set; } = BoundMode.Inclusive;

		/// <inherit />
		public override bool IsInRange(double value)
		{
			var isInRange = false;

			switch (LowerBoundMode)
			{
				case BoundMode.Inclusive:
					isInRange = value >= LowerBound;
					break;
				case BoundMode.Exclusive:
					isInRange = value > LowerBound;
					break;
			}

			switch (UpperBoundMode)
			{
				case BoundMode.Inclusive:
					isInRange &= value <= UpperBound;
					break;
				case BoundMode.Exclusive:
					isInRange &= value < UpperBound;
					break;
			}

			return isInRange;
		}
	}
}
