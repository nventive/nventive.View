using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	/// <summary>
	/// 
	/// </summary>
	public enum DoubleEqualityAccuracy
	{
		/// <summary>
		/// An accuracy of 0.1
		/// </summary>
		Minimal,

		/// <summary>
		/// An accuracy of 0.001
		/// </summary>
		Weak,

		/// <summary>
		/// An accuracy of 0.000001
		/// </summary>
		Normal,

		/// <summary>
		/// An accuracy of 0.000000001
		/// </summary>
		Precise,

		/// <summary>
		/// An accuracy of 0.000000000001
		/// </summary>
		Maximal
	}
}
