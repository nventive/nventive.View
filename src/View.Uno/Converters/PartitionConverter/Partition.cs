using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	/// <summary>
	/// The base class of a partition set.
	/// </summary>
	public abstract class Partition
	{
		/// <summary>
		/// Gets or sets the value to be returned if the value matches this partition set.
		/// </summary>
		public object InRangeValue { get; set; }

		/// <summary>
		/// When overriden in a derived class, allows to determine whether or not a value is contained within the partition
		/// </summary>
		/// <param name="value">The conversion value</param>
		/// <returns>A boolean value indicating if the value is in range</returns>
		public abstract bool IsInRange(double value);
	}
}