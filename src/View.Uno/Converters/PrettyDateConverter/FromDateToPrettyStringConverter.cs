using System;
using System.Reflection;
using System.Threading;
using System.Windows;
#if WINDOWS_UWP || __WASM__
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using GenericCulture = System.String;
#elif __ANDROID__ || __IOS__
using GenericCulture = System.String;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Chinook.View.Converters
{
	/// <summary>
	/// This converter is used to provide an easily readable and comprehensible string based on a TimeSpan, DateTime or DateTimeOffset.
	/// 
	/// NowGetter (Func<DateTime>) : Provides the converter with a specific way to fetch the current time.
	/// Formatter (IPrettyDateFormatter) : Provides the converter with a specific way to format the date.
	/// Mode (PrettyDateMode) : Changes the string so that it formats the string in the past or present. 
	/// E.G. 14 minutes vs. 14 minutes ago
	/// 
	/// NowGetter and Formatter have default implementations which provide expectable and standardized results.
	/// Mode is set to Past by default.
	/// If the value is null, an empty string will be returned
	/// 
	/// This converter is used to convert a TimeSpan, DateTime or DateTimeOffset to a string, E.G. to display when a file was uploaded.
	/// </summary>
	public class FromDateToPrettyStringConverter : IValueConverter, IAutoRefreshConverter
	{
		public static TimeSpan DefaultRefreshDelay = TimeSpan.FromSeconds(30);
		public static Func<DateTime> DefaultNowGetter = () => DateTime.Now;
		public static IPrettyDateFormatter DefaultFormatter = new PrettyDateFormatter();

		public Func<DateTime> NowGetter { get; set; }
		public IPrettyDateFormatter Formatter { get; set; }
		public PrettyDateMode Mode { get; set; }

		public FromDateToPrettyStringConverter()
		{
			NowGetter = DefaultNowGetter;
			Formatter = DefaultFormatter;
			Mode = PrettyDateMode.Past;
		}

		public object Convert(object value, Type targetType, object parameter, GenericCulture culture)
		{
			if (parameter != null)
			{
				throw new ArgumentException("This converter does not use any parameters");
			}

			if (value == null)
			{
				return string.Empty;
			}

			TimeSpan? elapsed = null;

			if (value is DateTime)
			{
				elapsed = NowGetter() - (DateTime)value;
			}
			else if (value is TimeSpan)
			{
				elapsed = (TimeSpan)value;
			}
			else if (value is DateTimeOffset)
			{
				elapsed = NowGetter() - (DateTimeOffset)value;
			}

			if (elapsed.HasValue)
			{
				return Formatter.Format(elapsed.Value, culture, Mode);
			}

			throw new ArgumentException("Value could not be properly converted to a PrettyDate.  Please use a Date/Time type value.");
		}

		public TimeSpan GetNextUpdateInterval(object value, Type targetType, object parameter, GenericCulture culture)
		{
			TimeSpan? e = null;

			if (value is DateTime)
			{
				e = NowGetter() - (DateTime)value;
			}
			else if (value is TimeSpan)
			{
				e = (TimeSpan)value;
			}

			if (e.HasValue)
			{
				var elapsed = e.Value;

				if (elapsed.TotalSeconds < 0)
				{
					elapsed = TimeSpan.FromSeconds(0);
				}

				if (elapsed.TotalDays >= 1)
				{
					return TimeSpan.FromHours(1);
				}
				if (elapsed.TotalHours >= 1)
				{
					return TimeSpan.FromMinutes(30);
				}
				if (elapsed.TotalMinutes >= 1)
				{
					return TimeSpan.FromMinutes(1);
				}
				if (elapsed.TotalSeconds >= 5)
				{
					return TimeSpan.FromSeconds(10);
				}
				return TimeSpan.FromSeconds(5);
			}
			else
			{
				return DefaultRefreshDelay;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture culture)
		{
			throw new NotSupportedException();
		}
	}
}
