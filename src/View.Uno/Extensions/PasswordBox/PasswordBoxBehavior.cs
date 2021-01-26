#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Windows.Input;
using Uno.Logging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Windows.System;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Nventive.View.Extensions
{
	[Windows.UI.Xaml.Data.Bindable]
	public partial class PasswordBoxBehavior
	{
		private static readonly Lazy<ILogger> _log = new Lazy<ILogger>(() => typeof(PasswordBoxBehavior).Log(), true);

#if __ANDROID__ || __IOS__
		#region NextControl DEPENDENCY PROPERTY

		/// <summary>
		/// Get the NextControl
		/// </summary>
		/// <param name="obj">TextBox</param>
		/// <returns>Value of the NextControlProperty</returns>
		public static Control GetNextControl(TextBox obj)
		{
			return obj.GetValue(NextControlProperty) as Control;
		}

		/// <summary>
		/// Set the NextControl
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
			DependencyProperty.RegisterAttached("NextControl", typeof(Control), typeof(PasswordBoxBehavior), new PropertyMetadata(null, OnNextControlChanged));

		/// <summary>
		/// Event raised when OnNextControl is changed
		/// </summary>
		/// <param name="d">TextBox</param>
		/// <param name="e">Dependency property event arguments</param>
		private static void OnNextControlChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var box = d as TextBox;
			if (box != null)
			{
				Observable.FromEventPattern<KeyEventHandler, KeyRoutedEventArgs>(h => box.KeyUp += h, h => box.KeyUp -= h, Scheduler.Immediate)
					.Where(_ => _.EventArgs.Key == VirtualKey.Enter && _.Sender as TextBox != null)
					.Do(_ => box_KeyUpNextControl(_.Sender, _.EventArgs))
					.Subscribe(_ => { }, err =>
					{
						var log = _log.Value;
						if (log.IsEnabled(LogLevel.Debug))
						{
							log.DebugFormat("{0}: PasswordBoxBehavior KeyUp EnterCommand error: {1} ; {2}", box.Name, err.Message, err.StackTrace);
						}
					});
			}
		}

		#endregion
#endif

		#region EnterCommand DEPENDENCY PROPERTY

		/// <summary>
		/// Get the EnterCommand
		/// </summary>
		/// <param name="obj">PasswordBox</param>
		/// <returns>Value of the EnterCommandProperty</returns>
		public static ICommand GetEnterCommand(PasswordBox obj)
		{
			return (ICommand)obj.GetValue(EnterCommandProperty);
		}

		/// <summary>
		/// Set the EnterCommand
		/// </summary>
		/// <param name="obj">PasswordBox</param>
		/// <param name="value">EnterCommand</param>
		public static void SetEnterCommand(PasswordBox obj, ICommand value)
		{
			obj.SetValue(EnterCommandProperty, value);
		}

		/// <summary>
		/// Property for the EnterCommand
		/// </summary>
		public static readonly DependencyProperty EnterCommandProperty =
			DependencyProperty.RegisterAttached("EnterCommand", typeof(ICommand), typeof(PasswordBoxBehavior), new PropertyMetadata(null, EnterCommandChanged));

		/// <summary>
		/// Event raised when EnterCommand is changed
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e">Dependency property event arguments</param>
		private static void EnterCommandChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			//For Windows the event is hooked up in Nventive.Views.Shared.Xaml.PasswordBehavior.cs
#if __ANDROID__ || __IOS__
			var box = d as PasswordBox;
			if (box != null && e.NewValue != null)
			{
				Observable
					.FromEventPattern<KeyEventHandler, object, KeyRoutedEventArgs>(
						h => box.KeyUp += h,
						h => box.KeyUp -= h,
						Scheduler.Immediate
					)
					.Where(_ => _.EventArgs.Key == VirtualKey.Enter)
					.SubscribeToElement(
						element: box,
						onNext: _ => TryExecuteEnterCommand(box, _.EventArgs),
						onError: err =>
						{
							var log = _log.Value;
							if (log.IsEnabled(LogLevel.Debug))
							{
								log.DebugFormat(
									"{0}: PasswordBoxBehavior KeyUp EnterCommand error: {1} ; {2}",
									box.Name,
									err.Message,
									err.StackTrace
								);
							}
						}
					);
			}
#endif
		}

		#endregion

		/// <summary>
		/// Set the DismissKeyboardOnEnter
		/// </summary>
		/// <param name="passwordBox">Target passwordBox</param>
		/// <param name="value"></param>
		#region Dismiss Keyboard On Enter Property
		public static void SetDismissKeyboardOnEnter(PasswordBox passwordBox, bool value)
		{
			passwordBox.SetValue(DismissKeyboardOnEnterProperty, value);
		}

		/// <summary>
		/// Get the DismissKeyboardOnEnter
		/// </summary>
		/// <param name="passwordBox">Target passwordBox</param>
		/// <returns></returns>
		public static bool GetDismissKeyboardOnEnter(PasswordBox passwordBox)
		{
			return (bool)passwordBox.GetValue(DismissKeyboardOnEnterProperty);
		}

		/// <summary>
		/// Property for DismissKeyboard
		/// </summary>
		public static readonly DependencyProperty DismissKeyboardOnEnterProperty = DependencyProperty.RegisterAttached(
			"DismissKeyboardOnEnter",
			typeof(bool),
			typeof(PasswordBoxBehavior),
			new PropertyMetadata(false, DismissKeyboardOnEnterChanged)
		);

		/// <summary>
		/// Dissmis Keyboard when pressing the dismiss button
		/// </summary>
		/// <param name="d">PasswordBox</param>
		/// <param name="e">DismissKeyboard</param>
		private static void DismissKeyboardOnEnterChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var box = d as PasswordBox;
			var dismissKeybaord = (bool)e.NewValue;

			if (box != null && dismissKeybaord)
			{
				Observable
					.FromEventPattern<KeyEventHandler, object, KeyRoutedEventArgs>(
						h => box.KeyUp += h,
						h => box.KeyUp -= h,
						Scheduler.Immediate
					)
					.Where(_ => _.EventArgs.Key == VirtualKey.Enter)
					.SubscribeToElement(
						element: box,
						onNext: _ => TryDismissKeyboard(box),
						onError: err =>
						{
							var log = _log.Value;
							if (log.IsEnabled(LogLevel.Debug))
							{
								log.DebugFormat(
									"{0}: PasswordBoxBehavior KeyUp DismissKeyboardOnEnter error: {1} ; {2}",
									box.Name,
									err.Message,
									err.StackTrace
								);
							}
						}
					);
			}
		}
		#endregion

#if __ANDROID__ || __IOS__
		/// <summary>
		/// Move focus to next control
		/// </summary>
		/// <param name="sender">Textbox of the password box</param>
		/// <param name="e">Event</param>
		private static void box_KeyUpNextControl(object sender, KeyRoutedEventArgs e)
		{
			var box = (TextBox)sender;
			if (e.Key == VirtualKey.Enter)
			{
				if (box != null)
				{
					var nextControl = GetNextControl(box);
#if __IOS__
					nextControl.BecomeFirstResponder();
					nextControl.StartBringIntoView();
#elif __ANDROID__
					nextControl.RequestFocus();
#endif
				}
			}
		}
#endif

		/// <summary>
		/// Executes the enter command (command executed when pressing the enter button)
		/// </summary>
		/// <param name="passwordBox">Target passwordBox</param>
		/// <param name="e">Event</param>
		private static void TryExecuteEnterCommand(PasswordBox passwordBox, KeyRoutedEventArgs e)
		{
			if (passwordBox == null)
			{
				return;
			}

			var cmd = GetEnterCommand(passwordBox);
			if (cmd == null)
			{
				return;
			}

			const object param = null;
			if (cmd.CanExecute(param))
			{
#if __ANDROID__ || __IOS__
				UpdateBinding(passwordBox);
#endif
				cmd.Execute(param);
				e.Handled = true;
			}
		}

#if __ANDROID__ || __IOS__
		/// <summary>
		/// Update the passwordBox's password property 
		/// </summary>
		/// <param name="passwordBox">Target passwordBox</param>
		private static void UpdateBinding(PasswordBox passwordBox)
		{
			passwordBox
				.GetBindingExpression(PasswordBox.PasswordProperty)?
				.UpdateSource(passwordBox.Password);
		}
#endif
		/// <summary>
		/// Dismisses the keyboard
		/// </summary>
		/// <param name="passwordBox">Target passwordBox</param>
		private static void TryDismissKeyboard(PasswordBox passwordBox)
		{
			if (passwordBox == null)
			{
				return;
			}

			if (GetDismissKeyboardOnEnter(passwordBox))
			{
				DismissKeyboard(passwordBox);
			}
		}

#if __ANDROID__
		/// <summary>
		/// Dismisses the keyboard
		/// </summary>
		/// <param name="passwordBox">Target passwordBox</param>
		private static void DismissKeyboard(PasswordBox passwordBox)
		{
			if (passwordBox == null)
			{
				return;
			}

			var inputManager = Android.App.Application.Context.GetSystemService(Android.Content.Context.InputMethodService)
				as Android.Views.InputMethods.InputMethodManager;
			
			inputManager?.HideSoftInputFromWindow(
				passwordBox.WindowToken,
				Android.Views.InputMethods.HideSoftInputFlags.None
			);
		}
#else
		/// <summary>
		/// Dismisses the keyboard
		/// </summary>
		/// <param name="passwordBox">Target passwordBox</param>
		private static void DismissKeyboard(PasswordBox passwordBox)
		{
		}
#endif
	}
}
#endif
