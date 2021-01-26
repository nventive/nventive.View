using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using GenericCulture = System.String;
#elif __ANDROID__ || __IOS__ || __WASM__
using Windows.UI.Xaml;
using GenericCulture = System.String;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
#else
using System.Windows.Data;
using System.Windows.Markup;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// A converter that chains multiple converters in order to provide a result.  Each provided will be 
	/// executed in the order they are provided.
	/// 
	/// ​​Converters (List<IValueConverter>) : The list of all converters to chain.
	/// 
	/// If no converters are provided, MultiConverter should return the initial value.
	/// 
	/// This converter may be used when we need multiple steps of conversion from the initial value to the result.
	/// </summary>
#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
	[ContentProperty(Name = "Converters")]
#else
	[ContentProperty("Converters")]
#endif
	public class MultiConverter : IValueConverter
	{
		public MultiConverter()
		{
			Converters = new List<IValueConverter>();
		}

		public List<IValueConverter> Converters { get; set; }

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		public object Convert(object value, Type targetType, object parameter, GenericCulture language)
		{
			if (value == null)
			{
				return null;
			}

			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}
			object currentValue = value;

			foreach (IValueConverter converter in this.Converters)
			{
				currentValue = converter.Convert(currentValue, typeof(object), parameter, language);
			}

			if (currentValue == null)
			{
				return null;
			}

#if WINDOWS_UWP
			if (!targetType.GetTypeInfo().IsAssignableFrom(currentValue.GetType().GetTypeInfo()))
#else
			if (!targetType.IsAssignableFrom(currentValue.GetType()))
#endif
			{
				throw new InvalidOperationException("Conversion chain does not result in the proper type.");
			}

			return currentValue;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture language)
		{
			if (value == null)
			{
				return null; // nothing to do with a null input parameter
			}

			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}

			object currentValue = value;

			for (int i = this.Converters.Count - 1; i >= 0; i--)
			{
				currentValue = this.Converters[i].ConvertBack(currentValue, targetType, parameter, language);
			}

			if (currentValue == null)
			{
				return null;
			}

#if WINDOWS_UWP
			if (!targetType.GetTypeInfo().IsAssignableFrom(currentValue.GetType().GetTypeInfo()))
#else
			if (!targetType.IsAssignableFrom(currentValue.GetType()))
#endif
			{
				throw new InvalidOperationException("Conversion chain does not result in the proper type.");
			}

			return currentValue;
		}
	}
}
