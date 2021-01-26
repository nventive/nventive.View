using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Converters
{
	internal static class DoubleEqualityAccuracyExtensions
	{
		public static double AsDouble(this DoubleEqualityAccuracy accuracy)
		{
			switch (accuracy)
			{
				case DoubleEqualityAccuracy.Minimal:
					return 0.1;
				
				case DoubleEqualityAccuracy.Weak:
					return 0.001;

				case DoubleEqualityAccuracy.Precise:
					return 0.000000001;

				case DoubleEqualityAccuracy.Maximal:
					return 0.000000000001;

				case DoubleEqualityAccuracy.Normal:
				default:
					return 0.000001;
			}
		}
	}
}
