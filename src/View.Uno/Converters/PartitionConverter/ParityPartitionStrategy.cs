using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	/// <summary>
	/// A partition strategy allowing to convert a numeric conversion value depending on its parity.
	/// </summary>
	public class ParityPartitionStrategy : PartitionStrategy
	{
		/// <summary>
		/// The converted value when the conversion value is even.
		/// </summary>
		public object EvenValue { get; set; }

		/// <summary>
		/// The converted value when the conversion value is odd.
		/// </summary>
		public object OddValue { get; set; }

		public override object Execute(double value)
		{
			var intValue = (int)value;

			// Ensures the received value is an int and tests for parity.
			// We use maximal accuracy to test is the double passed is actually an integer.
			return Math.Abs(value - (int)value) <= DoubleEqualityAccuracy.Maximal.AsDouble() &&
				intValue % 2 == 0 ? EvenValue : OddValue;
		}
	}
}
