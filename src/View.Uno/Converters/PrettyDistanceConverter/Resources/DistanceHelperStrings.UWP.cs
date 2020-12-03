#if WINDOWS_UWP
#pragma warning disable CS0618 // Type or member is obsolete
namespace Chinook.View.Converters.PrettyDistanceConverter.Resources
{
	using System;
	using System.Runtime.CompilerServices;
	using Windows.ApplicationModel.Resources.Core;

	internal class DistanceHelperStrings
	{
		private const string ResourcePrefix = "Chinook.View/DistanceHelperStrings/";

		private static ResourceMap _resourceMap;


		internal DistanceHelperStrings()
		{
		}

		internal static ResourceMap ResourceManager
		{
			get
			{
				if (ReferenceEquals(_resourceMap, null))
				{
					_resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
				}
				return _resourceMap;
			}
		}

		/// <summary>
		/// Looks up a localized string similar to about one mile.
		/// </summary>
		internal static string Imperial_AboutOneMileString => GetResource();
		internal static string Imperial_AboutOneMile_ShortString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to about one mile.
		/// </summary>
		internal static string Imperial_FeetFormatString => GetResource();
		internal static string Imperial_Feet_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0} feet.
		/// </summary>
		internal static string Imperial_HereFormatString => GetResource();
		internal static string Imperial_Here_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to a few feet.
		/// </summary>
		internal static string Imperial_MilesFormatString => GetResource();
		internal static string Imperial_Miles_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0.#} feet.
		/// </summary>
		internal static string Imperial_NearFeetFormatString => GetResource();
		internal static string Imperial_NearFeet_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0.#} miles.
		/// </summary>
		internal static string Imperial_NearMilesFormatString => GetResource();
		internal static string Imperial_NearMiles_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0} foot.
		/// </summary>
		internal static string Imperial_OneFootFormatString => GetResource();
		internal static string Imperial_OneFoot_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0} mile.
		/// </summary>
		internal static string Imperial_OneMileFormatString => GetResource();
		internal static string Imperial_OneMile_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0.#} mile.
		/// </summary>
		internal static string Imperial_UnderOneMileString => GetResource();
		internal static string Imperial_UnderOneMile_ShortString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to about one kilometer.
		/// </summary>
		internal static string Metric_AboutOneKilometerString => GetResource();
		internal static string Metric_AboutOneKilometer_ShortString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to less than a meter.
		/// </summary>
		internal static string Metric_HereFormatString => GetResource();
		internal static string Metric_Here_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0} kilometers.
		/// </summary>
		internal static string Metric_KilometersFormatString => GetResource();
		internal static string Metric_Kilometers_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0} meters.
		/// </summary>
		internal static string Metric_MetersFormatString => GetResource();
		internal static string Metric_Meters_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0.#} kilometers.
		/// </summary>
		internal static string Metric_NearKilometersFormatString => GetResource();
		internal static string Metric_NearKilometers_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0.#} meters.
		/// </summary>
		internal static string Metric_NearMetersFormatString => GetResource();
		internal static string Metric_NearMeters_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0} kilometer.
		/// </summary>
		internal static string Metric_OneKilometerFormatString => GetResource();
		internal static string Metric_OneKilometer_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0} meter.
		/// </summary>
		internal static string Metric_OneMeterFormatString => GetResource();
		internal static string Metric_OneMeter_ShortFormatString => GetResource();

		/// <summary>
		/// Looks up a localized string similar to {0:0.#} kilometer.
		/// </summary>
		internal static string Metric_UnderOneKilometerString => GetResource();
		internal static string Metric_UnderOneKilometer_ShortString => GetResource();

		internal static string GetResource([CallerMemberName] string callerMemberName = "")
		{
			return ResourceManager.GetValue(ResourcePrefix + callerMemberName).ValueAsString;
		}
	}
}
#endif
