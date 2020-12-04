#if WINDOWS_UWP || HAS_WINUI || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
#if HAS_WINUI
using Microsoft.UI.Xaml.Input;
#else
using Windows.UI.Xaml.Input;
#endif

namespace Chinook.View.Converters
{
	/// <summary>
	/// This converter is used to turn an InputScopeNameValue into an InputScope
	/// 
	/// This converter is particularly useful with StructuredContent
	/// </summary>
	public class FromInputScopeNameToInputScopeConverter : ConverterBase
	{
		protected override object Convert(object value, Type targetType, object parameter)
		{
			var scope = new InputScope();
			InputScopeName scopeName = null;

			if (value == null)
			{
				scopeName = InputScopeNameWithValue(InputScopeNameValue.Default);
			}
			else
			{
				scopeName = InputScopeNameWithValue((InputScopeNameValue)value);
			}

			scope.Names.Add(scopeName);

			return scope;
		}

		private InputScopeName InputScopeNameWithValue(InputScopeNameValue value)
		{
			return new InputScopeName(value);
		}
	}
}
#endif
