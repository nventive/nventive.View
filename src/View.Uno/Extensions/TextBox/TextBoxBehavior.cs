#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Windows.Input;
using Uno.Logging;
using System.Text;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Nventive.View.Extensions
{
	[Windows.UI.Xaml.Data.Bindable]
	public partial class TextBoxBehavior
	{
		private static readonly Lazy<ILogger> _log = new Lazy<ILogger>(() => typeof(TextBoxBehavior).Log(), true);

		#region NextControl DEPENDENCY PROPERTY

		/// <summary>
		/// Get NextControl
		/// </summary>
		/// <param name="obj">TextBox</param>
		/// <returns>Value of the NextControlProperty</returns>
		public static Control GetNextControl(TextBox obj)
		{
			return obj.GetValue(NextControlProperty) as Control;
		}

		/// <summary>
		/// Set NextControl
		/// </summary>
		/// <param name="obj">TextBox</param>
		/// <param name="value">NextControl</param>
		public static void SetNextControl(TextBox obj, Control value)
		{
			obj.SetValue(NextControlProperty, value);
		}

		/// <summary>
		/// Property for the EnterCommand
		/// </summary>
		public static readonly DependencyProperty NextControlProperty =
			DependencyProperty.RegisterAttached("NextControl", typeof(Control), typeof(TextBoxBehavior), new PropertyMetadata(null, OnNextControlChanged));

		private static void OnNextControlChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var textBox = sender as TextBox;

			if (textBox != null)
			{
				textBox.KeyUp += OnNextControlKeyUp;
			}
		}

		private static void OnNextControlKeyUp(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
			{
				FocusNextControl(sender as TextBox);
			}
		}
		#endregion

		#region EnterCommand DEPENDENCY PROPERTY

		/// <summary>
		/// Get the EnterCommandProperty
		/// </summary>
		/// <param name="obj">TextBox</param>
		/// <returns>Value of the EnterCommandProperty</returns>
		public static ICommand GetEnterCommand(TextBox obj)
		{
			return (ICommand)obj.GetValue(EnterCommandProperty);
		}

		/// <summary>
		/// Set the EnterCommandProperty
		/// </summary>
		/// <param name="obj">TextBox</param>
		/// <param name="value">EnterCommand</param>
		public static void SetEnterCommand(TextBox obj, ICommand value)
		{
			obj.SetValue(EnterCommandProperty, value);
		}

		/// <summary>
		/// Property for the EnterCommand
		/// </summary>
		public static readonly DependencyProperty EnterCommandProperty =
			DependencyProperty.RegisterAttached("EnterCommand", typeof(ICommand), typeof(TextBoxBehavior), new PropertyMetadata(null, OnEnterCommandChanged));

		/// <summary>
		/// Event raised when EnterCommand is changed
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e">Dependency property event arguments</param>
		private static void OnEnterCommandChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var box = d as TextBox;
			if (box != null)
			{
				box.KeyUp += OnEnterCommandKeyUp;
			}
		}

		private static void OnEnterCommandKeyUp(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
			{
				TryExecuteEnterCommand(sender as TextBox, e);
			}
		}

		#endregion

		/// <summary>
		/// Set the DismissKeyboardOnEnter
		/// </summary>
		#region Dismiss Keyboard On Enter Property
		public static void SetDismissKeyboardOnEnter(TextBox textBox, bool value)
		{
			textBox.SetValue(DismissKeyboardOnEnterProperty, value);
		}

		/// <summary>
		/// Get the DismissKeyboardOnEnter
		/// </summary>
		public static bool GetDismissKeyboardOnEnter(TextBox textBox)
		{
			return (bool)textBox.GetValue(DismissKeyboardOnEnterProperty);
		}

		/// <summary>
		/// Property for DismissKeyboard
		/// </summary>
		public static readonly DependencyProperty DismissKeyboardOnEnterProperty = DependencyProperty.RegisterAttached(
			"DismissKeyboardOnEnter",
			typeof(bool),
			typeof(TextBoxBehavior),
			new PropertyMetadata(false, DismissKeyboardOnEnterChanged)
		);

		/// <summary>
		/// Dissmis Keyboard when pressing the dismiss button
		/// </summary>
		private static void DismissKeyboardOnEnterChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var textBox = d as TextBox;
			var dismissKeybaord = (bool)e.NewValue;

			if (textBox != null && dismissKeybaord)
			{
				textBox.KeyUp += OnDismissKeyUp;
			}
		}

		private static void OnDismissKeyUp(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == VirtualKey.Enter)
			{
				TryDismissKeyboard(sender as TextBox);
			}
		}
		#endregion
	}
}
#endif
