#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Uno.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Chinook.View.Extensions
{
	public class MultipleTapExtension
	{
		private const string TapCounterKey = nameof(TapCounterKey);

		private static WeakAttachedDictionary<DependencyObject, string> _tapCounters 
			= new WeakAttachedDictionary<DependencyObject, string>();

		public static ICommand GetCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(CommandProperty);
		}

		public static void SetCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(CommandProperty, value);
		}

		/// <summary>
		/// Command to execute.
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MultipleTapExtension), new PropertyMetadata(default(ICommand), OnCommandChanged));

		public static object GetCommandParameter(DependencyObject obj)
		{
			return obj.GetValue(CommandParameterProperty);
		}

		public static void SetCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(CommandParameterProperty, value);
		}
		
		/// <summary>
		/// Command parameter for the <see cref="CommandProperty"/>.
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(MultipleTapExtension), new PropertyMetadata(default(object)));

		public static int GetTapCount(DependencyObject obj)
		{
			return (int)obj.GetValue(TapCountProperty);
		}

		public static void SetTapCount(DependencyObject obj, int value)
		{
			obj.SetValue(TapCountProperty, value);
		}

		/// <summary>
		/// Number of taps required to execute the <see cref="CommandProperty"/>.
		/// </summary>
		public static readonly DependencyProperty TapCountProperty =
			DependencyProperty.RegisterAttached("TapCount", typeof(int), typeof(MultipleTapExtension), new PropertyMetadata(default(int)));

		private static void OnCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var frameworkElement = (FrameworkElement)o;

			frameworkElement.PointerPressed -= ExecuteCommand;

			// Reset the tap counter.
			_tapCounters.SetValue(frameworkElement, TapCounterKey, 0);

			if (e.NewValue != null)
			{
				frameworkElement.PointerPressed += ExecuteCommand;
			}
		}
		
		private static void ExecuteCommand(object sender, PointerRoutedEventArgs args)
		{
			var dependencyObject = sender as DependencyObject;

			var command = GetCommand(dependencyObject);
			var commandParameter = GetCommandParameter(dependencyObject);
			var tapCount = GetTapCount(dependencyObject);

			var taps = _tapCounters.GetValue<int>(dependencyObject, TapCounterKey) + 1;

			if (taps >= tapCount)
			{
				command.Execute(commandParameter);
				taps = 0;
			}

			_tapCounters.SetValue(dependencyObject, TapCounterKey, taps);
		}
	}
}
#endif
