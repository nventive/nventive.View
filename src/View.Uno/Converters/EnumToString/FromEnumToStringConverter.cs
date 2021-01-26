using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
#if WINDOWS_UWP
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using GenericCulture = System.String;
#elif __ANDROID__ || __IOS__ || __WASM__
using Uno.UI;
using GenericCulture = System.String;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using GenericCulture = System.Globalization.CultureInfo;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// Converts a enum value to a readable localizable string using the resource manager.
	/// 
	/// CharacterCasing (CharacterCasingOption) : Casing the the resulting string
	/// 
	/// The default CharacterCasing is None, which will output the localized resource as is.
	/// 
	/// This may be used whenever an enum value needs to be displayed on the screen.
	/// 
	/// Needs to be injected from code behind, cannot be used in pure XAML.
	/// </summary>
	public class FromEnumToStringConverter : IValueConverter
	{
		private Func<string, string> _resourceProvider;

		public FromEnumToStringConverter(Func<string, string> resourceProvider)
		{
			_resourceProvider = resourceProvider;
			this.CharacterCasing = CharacterCasingOption.None;
		}

		public CharacterCasingOption CharacterCasing { get; set; }

		public object Convert(object value, Type targetType, object parameter, GenericCulture culture)
		{
			if (parameter != null)
			{
				throw new ArgumentException("This converter does not use any parameters");
			}

			if (value == null || (targetType != typeof(string) && targetType != typeof(object)))
			{
				return "";
			}

			var output = GetString(value.GetType().Name + "_" + value);

			switch (this.CharacterCasing)
			{
				case CharacterCasingOption.UpperCase:
#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
					output = output.ToUpper();
#else
					output = output.ToUpper(culture);
#endif
					break;

				case CharacterCasingOption.LowerCase:
#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
					output = output.ToLower();
#else
					output = output.ToLower(culture);
#endif
					break;
			}

			return output;
		}

		private string GetString(string key)
		{
			return _resourceProvider(key) ?? string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, GenericCulture culture)
		{
			throw new NotSupportedException();
		}
	}

	public enum CharacterCasingOption
	{
		None,
		LowerCase,
		UpperCase
	}
}
