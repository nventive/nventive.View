using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

#if WINDOWS_UWP
using Windows.UI.Xaml;
#elif __ANDROID__ || __IOS__ || __WASM__
using Visibility = Windows.UI.Xaml.Visibility;
#else
using System.Windows;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter outputs a visibility value based on the presence of any items in an enumerable
	/// 
	/// VisibilityOnEnumerableHasAny (VisibilityOnEnumerableHasAny) : The visibility that should be returned
	/// when the enumerable has items.
	/// 
	/// By default, VisibilityOnEnumerableHasAny is set to Visible.
	/// 
	/// This may be used to show or hide a list based on the presence or absence of data.
	/// </summary>
	public class FromEnumerableHasAnyToVisibilityConverter : ConverterBase
	{
		public FromEnumerableHasAnyToVisibilityConverter()
		{
			this.VisibilityOnEnumerableHasAny = Converters.VisibilityOnEnumerableHasAny.Visible;
		}

		public VisibilityOnEnumerableHasAny VisibilityOnEnumerableHasAny { get; set; }

		[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "Not for end user")]
		protected override object Convert(object value, Type targetType, object parameter)
		{
			if (parameter != null)
			{
				throw new ArgumentException($"This converter does not use any parameters. You should remove \"{parameter}\" passed as parameter.");
			}

			bool inverse = this.VisibilityOnEnumerableHasAny == VisibilityOnEnumerableHasAny.Collapsed;
			
			Visibility visibilityOnTrue = (!inverse) ? Visibility.Visible : Visibility.Collapsed;
			Visibility visibilityOnFalse = (!inverse) ? Visibility.Collapsed : Visibility.Visible;

			IEnumerable enumerableValue = value as IEnumerable;

			if (value != null && enumerableValue == null)
			{
				throw new ArgumentException($"Converter value (of type {value.GetType().FullName}) needs to be an IEnumerable.");
			}

			var valueToConvert = enumerableValue?.Cast<object>().Any() ?? false;

			return valueToConvert ? visibilityOnTrue : visibilityOnFalse;
		}
	}

	public enum VisibilityOnEnumerableHasAny
	{
		Visible,
		Collapsed
	}
}
