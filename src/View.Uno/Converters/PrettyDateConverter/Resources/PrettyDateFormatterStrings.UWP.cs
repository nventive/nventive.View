#if WINDOWS_UWP
#pragma warning disable CS0618 // Type or member is obsolete
namespace Chinook.View.Converters.PrettyDateConverter.Resources
{
	using System;
	using System.Collections.Generic;
	using Windows.ApplicationModel.Resources;
	using Windows.ApplicationModel.Resources.Core;

	internal class PrettyDateFormatterStrings 
	{
		private const string ResourcePrefix = "Chinook.View/PrettyDateFormatterStrings/";

		private static ResourceMap resourceMan;

		internal PrettyDateFormatterStrings()
		{
		}

		internal static ResourceMap ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					resourceMan = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
				}
				return resourceMan;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} ago.
		/// </summary>
		internal static string CompleteDateFormat
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "CompleteDateFormat").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} day.
		/// </summary>
		internal static string DayFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "DayFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} days.
		/// </summary>
		internal static string DaysFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "DaysFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} hour.
		/// </summary>
		internal static string HourFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "HourFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} hours.
		/// </summary>
		internal static string HoursFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "HoursFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} minute.
		/// </summary>
		internal static string MinuteFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "MinuteFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} minutes.
		/// </summary>
		internal static string MinutesFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "MinutesFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to now.
		/// </summary>
		internal static string Now
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "Now").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} second.
		/// </summary>
		internal static string SecondFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "SecondFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} seconds.
		/// </summary>
		internal static string SecondsFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "SecondsFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} week.
		/// </summary>
		internal static string WeekFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "WeekFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} weeks.
		/// </summary>
		internal static string WeeksFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "WeeksFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} year.
		/// </summary>
		internal static string YearFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "YearFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to {0} years.
		/// </summary>
		internal static string YearsFormatString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "YearsFormatString").ValueAsString;
			}
		}

		/// <summary>
		///   Looks up a localized string similar to yesterday.
		/// </summary>
		internal static string YesterdayString
		{
			get
			{
				return ResourceManager.GetValue(ResourcePrefix + "YesterdayString").ValueAsString;
			}
		}
	}
}
#endif
