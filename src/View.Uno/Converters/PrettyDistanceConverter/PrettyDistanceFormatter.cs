using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Chinook.View.Converters
{
	public interface IPrettyDistanceFormatter
	{
		string Format(double distanceInMeters, CultureInfo cultureInfo = null, DistanceUnit distanceOutType = DistanceUnit.CurrentCulture);
	}

	public class PrettyDistanceFormatter : IPrettyDistanceFormatter
	{
		public DistanceUnitFormat DistanceUnitFormat { get; set; } = DistanceUnitFormat.Long;

		/// <summary>
		/// When this is true, the format will always be a number value instead of a text (e.g. Less than a meter)
		/// </summary>
		public bool ForceKmOrMiles { get; set; } = false;

		public string Format(double distanceInMeters, CultureInfo cultureInfo = null, DistanceUnit distanceOutType = DistanceUnit.CurrentCulture)
		{
			return DistanceHelper.GetPrettyDistance(distanceInMeters, cultureInfo, distanceOutType, ForceKmOrMiles, DistanceUnitFormat);
		}
	}
}