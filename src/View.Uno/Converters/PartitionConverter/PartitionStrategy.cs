using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	/// <summary>
	/// An extensibility class allowing to create an user defined partition strategy over the double set.
	/// </summary>
	public abstract class PartitionStrategy
	{
		/// <summary>
		/// When overriden in a derived class, allows to perform calculation over a double value in order to select a value for the <see cref="PartitionConverter"/> to return
		/// </summary>
		/// <param name="value">The conversion value</param>
		/// <returns>The converted value</returns>
		public abstract object Execute(double value);
	}
}
