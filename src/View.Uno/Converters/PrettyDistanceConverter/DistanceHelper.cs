using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Nventive.View.Converters.PrettyDistanceConverter.Resources;

namespace Nventive.View.Converters
{
	public class DistanceHelper
	{
		public const double FeetToMeterRatio = 3.2808399;
		public const int MileToFootRatio = 5280;
		public const double MetersToMile = 1609.344d;

		/// <summary>
		/// This static property can be used to override the current culture for determination of the measurement units to be used.
		/// </summary>
		public static DistanceUnit DistanceUnit { get; set; }

		/// <summary>
		/// Convert a distance from meters into a formatted string that shows accurate measurement units, rounded for meaninful display to a end-user. 
		/// </summary>
		/// <param name="distanceInMeters">Input distance </param>
		/// <param name="cultureInfo">Allow specifying a culture that will impact formatting and language of resulting pretty distance. This culture will also be used to determine the measurement units to use, if not ovverriden by <paramref name="distanceOutType"/> or by member DistanceUnit. The default value for this parameter is null, which lets the current culture of application decide.</param>
		/// <param name="distanceOutType">Allow forcing the measurement unit used for the resulting pretty distance. If null or not specified, the value specified in member DistanceUnit will be used (or the culture [current or specified], if DistanceUnit is not specifed).</param>
		/// <param name="forceKmOrMiles">Determines if the resulting pretty distance will be forced into km or miles. This can be used to prevent "meters", "foot" or other small units.</param>
		/// <returns></returns>
		public static string GetPrettyDistance(double distanceInMeters, CultureInfo cultureInfo = null, DistanceUnit distanceOutType = DistanceUnit.CurrentCulture, bool forceKmOrMiles = false, DistanceUnitFormat distanceUnitFormat = DistanceUnitFormat.Long)
		{
			var effectiveCultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;
			var effectiveDistanceUnit = distanceOutType != DistanceUnit.CurrentCulture ? distanceOutType : GetDistanceUnit(effectiveCultureInfo);

			if (effectiveDistanceUnit == DistanceUnit.Metric)
			{
				return GetMetricDistance(distanceInMeters, forceKmOrMiles, effectiveCultureInfo, distanceUnitFormat);
			}
			else
			{
				return GetImperialDistance(distanceInMeters, forceKmOrMiles, effectiveCultureInfo, distanceUnitFormat);
			}
		}

		private static string GetMetricDistance(double distanceInMeters, bool forceKmOrMiles, CultureInfo effectiveCultureInfo, DistanceUnitFormat distanceUnitFormat)
		{
			string format;

			var useLongFormat = distanceUnitFormat == DistanceUnitFormat.Long;

			if (!forceKmOrMiles)
			{
				if (distanceInMeters < 1) // Here
				{
					return useLongFormat
						? DistanceHelperStrings.Metric_HereFormatString
						: DistanceHelperStrings.Metric_Here_ShortFormatString;
				}

				if (Math.Round(distanceInMeters) == 1) // 1 meter
				{
					format = useLongFormat
						? DistanceHelperStrings.Metric_OneMeterFormatString
						: DistanceHelperStrings.Metric_OneMeter_ShortFormatString;

					return String.Format(effectiveCultureInfo, format, distanceInMeters);
				}

				if (distanceInMeters < 10) // 9.5 meters
				{
					format = useLongFormat
						? DistanceHelperStrings.Metric_NearMetersFormatString
						: DistanceHelperStrings.Metric_NearMeters_ShortFormatString;

					return String.Format(effectiveCultureInfo, format, distanceInMeters);
				}

				if (distanceInMeters < 300) // 145 meters
				{
					format = useLongFormat
						? DistanceHelperStrings.Metric_MetersFormatString
						: DistanceHelperStrings.Metric_Meters_ShortFormatString;

					return String.Format(effectiveCultureInfo, format, distanceInMeters);
				}

				if (distanceInMeters < 800) // x00 meters
				{
					format = useLongFormat
						? DistanceHelperStrings.Metric_MetersFormatString
						: DistanceHelperStrings.Metric_Meters_ShortFormatString;

					return String.Format(effectiveCultureInfo, format, Math.Round(distanceInMeters / 100) * 100);
				}

				if (Math.Round(distanceInMeters / 1000) == 1) // 1 Kilometer
				{
					format = useLongFormat
						? DistanceHelperStrings.Metric_OneKilometerFormatString
						: DistanceHelperStrings.Metric_OneKilometer_ShortFormatString;

					return String.Format(effectiveCultureInfo, format, distanceInMeters / 1000);
				}
			}
			if (distanceInMeters < 1000)
			{
				format = useLongFormat
					? DistanceHelperStrings.Metric_UnderOneKilometerString
					: DistanceHelperStrings.Metric_UnderOneKilometer_ShortString;

				return String.Format(effectiveCultureInfo, format, distanceInMeters / 1000);
			}

			if (distanceInMeters < 10000) // 9.5 kilometers
			{
				format = useLongFormat
					? DistanceHelperStrings.Metric_NearKilometersFormatString
					: DistanceHelperStrings.Metric_NearKilometers_ShortFormatString;

				return String.Format(effectiveCultureInfo, format, distanceInMeters / 1000);
			}

			format = useLongFormat
				? DistanceHelperStrings.Metric_KilometersFormatString
				: DistanceHelperStrings.Metric_Kilometers_ShortFormatString;

			return String.Format(effectiveCultureInfo, format, distanceInMeters / 1000);
		}

		private static string GetImperialDistance(double distanceInMeters, bool forceKmOrMiles, CultureInfo effectiveCultureInfo, DistanceUnitFormat distanceUnitFormat)
		{
			string format;

			var useLongFormat = distanceUnitFormat == DistanceUnitFormat.Long;
			double distanceInFeet = Math.Round(distanceInMeters * FeetToMeterRatio);

			if (!forceKmOrMiles)
			{
				if (distanceInFeet < 5) // Here
				{
					return useLongFormat
						? DistanceHelperStrings.Imperial_HereFormatString
						: DistanceHelperStrings.Imperial_Here_ShortFormatString;
				}

				if (distanceInFeet < 400) // 25 feet
				{
					format = useLongFormat
						? DistanceHelperStrings.Imperial_NearFeetFormatString
						: DistanceHelperStrings.Imperial_NearFeet_ShortFormatString;

					return String.Format(effectiveCultureInfo, format, distanceInFeet);
				}

				if (distanceInFeet < MileToFootRatio * 0.75) // 800 feet
				{
					format = useLongFormat
						? DistanceHelperStrings.Imperial_FeetFormatString
						: DistanceHelperStrings.Imperial_Feet_ShortFormatString;

					return String.Format(effectiveCultureInfo, format, Math.Round(distanceInFeet / 100) * 100);
				}
			}

			var distanceInMiles = distanceInFeet / MileToFootRatio;

			if (Math.Round(distanceInMiles) == 1) // 1 mile
			{
				format = useLongFormat
						? DistanceHelperStrings.Imperial_OneMileFormatString
						: DistanceHelperStrings.Imperial_OneMile_ShortFormatString;

				return String.Format(effectiveCultureInfo, format, distanceInMiles);
			}

			if (distanceInFeet < MileToFootRatio && forceKmOrMiles)
			{
				format = useLongFormat
						? DistanceHelperStrings.Imperial_UnderOneMileString
						: DistanceHelperStrings.Imperial_UnderOneMile_ShortString;

				return String.Format(effectiveCultureInfo, format, distanceInMiles);
			}

			if (distanceInFeet < (MileToFootRatio + 500) && !forceKmOrMiles) // about 1 mile
			{
				format = useLongFormat
						? DistanceHelperStrings.Imperial_AboutOneMileString
						: DistanceHelperStrings.Imperial_AboutOneMile_ShortString;

				return String.Format(effectiveCultureInfo, format);
			}

			if (distanceInFeet < (MileToFootRatio * 10)) // 9.5 miles
			{
				format = useLongFormat
						? DistanceHelperStrings.Imperial_NearMilesFormatString
						: DistanceHelperStrings.Imperial_NearMiles_ShortFormatString;

				return String.Format(effectiveCultureInfo, format, distanceInMiles);
			}

			format = useLongFormat
				? DistanceHelperStrings.Imperial_MilesFormatString
				: DistanceHelperStrings.Imperial_Miles_ShortFormatString;

			return String.Format(effectiveCultureInfo, format, distanceInMiles);
		}

		/// <summary>
		/// This method is used to convert a speed in meters per second into m/h or km/h
		/// </summary>
		/// <param name="metersPerSecond">speed in meters per second</param>
		/// <param name="cultureInfo"> </param>
		/// <returns>Speed in m/h or km/h</returns>
		public static double GetSpeed(double metersPerSecond, CultureInfo cultureInfo = null)
		{
			return 3600 *
				   (IsMetric(cultureInfo) ? metersPerSecond / 1000 : metersPerSecond / MetersToMile);
		}

		private static DistanceUnit GetDistanceUnit(CultureInfo cultureInfo = null)
		{
			var effectiveCultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

			return DistanceUnit != DistanceUnit.CurrentCulture
					   ? DistanceUnit
					   : (new RegionInfo(effectiveCultureInfo.Name).IsMetric
							  ? DistanceUnit.Metric
							  : DistanceUnit.Imperial);
		}

		private static bool IsMetric(CultureInfo cultureInfo = null)
		{
			return GetDistanceUnit(cultureInfo) == DistanceUnit.Metric;
		}
	}
}
