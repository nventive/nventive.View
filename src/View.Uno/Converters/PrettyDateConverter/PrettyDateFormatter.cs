using System;
using System.Collections.Generic;
using System.Globalization;
using Chinook.View.Converters.PrettyDateConverter.Resources;
#if !WINDOWS_UWP && !HAS_WINUI && !__ANDROID__ && !__IOS__ && !__WASM__
using Uno.Localisation;
#else
using CultureInfo = System.String;
#endif
namespace Chinook.View.Converters
{
	public interface IPrettyDateFormatter
	{
		string Format(TimeSpan elapsed, CultureInfo culture, PrettyDateMode mode = PrettyDateMode.Past);
	}

	public enum PrettyDateMode
	{
		Past = 0,
		Neutral = 1
	}

	public class PrettyDateFormatter : IPrettyDateFormatter
	{
		private const string separator = " ";
		public double NumberOfHoursBeforeDayFormatting { get; set; }

		public PrettyDateFormatter()
		{
			NumberOfHoursBeforeDayFormatting = 48.0d; //To avoid breaking changes
		}

		public string Format(TimeSpan elapsed, CultureInfo culture, PrettyDateMode mode = PrettyDateMode.Past)
		{
			if (elapsed.TotalSeconds < 1)
			{
				elapsed = TimeSpan.Zero;
			}

			//if (dateTime.Age(now) >= 1)
			//{
			//    return FormatString(culture, years: dateTime.Age(now));
			//}
			//else if (elapsed.TotalWeeks() >= 2)
			//{
			//    return FormatString(culture, weeks: (int)elapsed.TotalWeeks());
			//}
			//else if (elapsed.TotalHours >= 3 && dateTime.IsYesterday(now))
			//{
			//    return YesterdayString;
			//}
			if (elapsed.TotalHours >= NumberOfHoursBeforeDayFormatting) //To avoid breaking changes
			{
				return FormatString(mode, culture, days: (int)elapsed.TotalDays);
			}
			if (elapsed.TotalHours >= 1)
			{
				return FormatString(mode, culture, hours: (int)elapsed.TotalHours);
			}
			if (elapsed.TotalMinutes >= 1)
			{
				return FormatString(mode, culture, minutes: (int)elapsed.TotalMinutes);
			}
			if (elapsed.TotalSeconds >= 5)
			{
				return FormatString(mode, culture, seconds: (int)elapsed.TotalSeconds);
			}
			return FormatString(mode, culture);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "CA is wrong here")]
		private static string FormatString(PrettyDateMode mode, CultureInfo culture, int? years = null, int? weeks = null, int? days = null, int? hours = null, int? minutes = null, int? seconds = null)
		{
#if !WINDOWS_UWP && !__ANDROID__ && !__IOS__ && !__WASM__
			using (culture == null ? null : new CultureContext(culture))
			{
#endif
				if (!years.HasValue && !weeks.HasValue && !days.HasValue && !hours.HasValue && !minutes.HasValue &&
					!seconds.HasValue)
				{
					return PrettyDateFormatterStrings.Now;
				}

				var parts = new List<string>();

				if (years.HasValue && years != 0)
				{
					parts.Add(GetPart(culture, years.Value, PrettyDateFormatterStrings.YearFormatString,
									  PrettyDateFormatterStrings.YearsFormatString));
				}
				if (weeks.HasValue && weeks != 0)
				{
					parts.Add(GetPart(culture, weeks.Value, PrettyDateFormatterStrings.WeekFormatString,
									  PrettyDateFormatterStrings.WeeksFormatString));
				}
				if (days.HasValue && days != 0)
				{
					parts.Add(GetPart(culture, days.Value, PrettyDateFormatterStrings.DayFormatString,
									  PrettyDateFormatterStrings.DaysFormatString));
				}
				if (hours.HasValue && hours != 0)
				{
					parts.Add(GetPart(culture, hours.Value, PrettyDateFormatterStrings.HourFormatString,
									  PrettyDateFormatterStrings.HoursFormatString));
				}
				if (minutes.HasValue && minutes != 0)
				{
					parts.Add(GetPart(culture, minutes.Value, PrettyDateFormatterStrings.MinuteFormatString,
									  PrettyDateFormatterStrings.MinutesFormatString));
				}
				if (seconds.HasValue && seconds != 0)
				{
					parts.Add(GetPart(culture, seconds.Value, PrettyDateFormatterStrings.SecondFormatString,
									  PrettyDateFormatterStrings.SecondsFormatString));
				}

				var result = String.Join(separator, parts.ToArray());

				switch (mode)
				{
					case PrettyDateMode.Neutral:
						return result;
					case PrettyDateMode.Past:
#if !WINDOWS_UWP && !__ANDROID__ && !__IOS__ && !__WASM__
						return String.Format(culture, PrettyDateFormatterStrings.CompleteDateFormat, result);
#else
					return String.Format(PrettyDateFormatterStrings.CompleteDateFormat, result);
#endif
					default:
						throw new ArgumentException("Invalid mode", "mode");
				}
#if !WINDOWS_UWP && !__ANDROID__ && !__IOS__ && !__WASM__
			}
#endif
		}

		private static string GetPart(CultureInfo culture, int value, string singular, string plural)
		{
#if !WINDOWS_UWP && !__ANDROID__ && !__IOS__ && !__WASM__
			return String.Format(culture, value > 1 ? plural : singular, value);
#else
			return String.Format(value > 1 ? plural : singular, value);
#endif
		}
	}
}
