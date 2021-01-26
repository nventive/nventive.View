using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	#region DiscretePartition
	/// <summary>
	/// Allows to match an exact value of a partition. An <see cref="Accuracy"/> can be provided.
	/// </summary>
	/// <remarks>
	/// If no <see cref="DoubleEqualityAccuracy"/> is provided, the <see cref="DoubleEqualityAccuracy.Normal"/> will be used.
	/// </remarks>
	public class DiscretePartition : Partition
	{
		/// <summary>
		/// The exact value to match the conversion value for it to be considered in range.
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		/// The accuracy for the double comparison.
		/// </summary>
		public DoubleEqualityAccuracy Accuracy { get; set; } = DoubleEqualityAccuracy.Normal;

		/// <inherit />
		public override bool IsInRange(double value) => Math.Abs(value - Value) <= Accuracy.AsDouble();
	}
	#endregion
}
