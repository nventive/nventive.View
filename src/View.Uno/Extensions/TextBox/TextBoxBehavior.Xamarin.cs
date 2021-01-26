#if __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Nventive.View.Extensions
{
    public partial class TextBoxBehavior
    {
		/// <summary>
		/// Move focus to next control
		/// </summary>
		/// <param name="sender">Textbox</param>
		/// <param name="e">Event</param>
		private static void FocusNextControl(TextBox textBox)
		{
				if (textBox != null)
				{
					var nextControl = GetNextControl(textBox);
#if __IOS__
					nextControl.BecomeFirstResponder();
					nextControl.StartBringIntoView();
#elif __ANDROID__
					nextControl.RequestFocus();
#endif
				}
		}

		/// <summary>
		/// Executes the enter command (command executed when pressing the enter button)
		/// </summary>
		/// <param name="sender">Target TextBox</param>
		/// <param name="e">Event</param>
		private static void TryExecuteEnterCommand(object sender, KeyRoutedEventArgs e)
		{
			var box = sender as TextBox;
			if (box == null)
			{
				return;
			}

			var cmd = GetEnterCommand(box);
			if (cmd == null)
			{
				return;
			}

			const object param = null;
			if (cmd.CanExecute(param))
			{
				UpdateBinding(box);

				cmd.Execute(param);
			}
		}

		/// <summary>
		/// Update the TextBox's Text property 
		/// </summary>
		/// <param name="textBox">Target TextBox</param>
		private static void UpdateBinding(TextBox textBox)
		{
			textBox
				.GetBindingExpression(TextBox.TextProperty)?
				.UpdateSource(textBox.Text);
		}

		/// <summary>
		/// Dismisses the keyboard
		/// </summary>
		/// <param name="textBoxBox">Target TextBoxBox</param>
		/// #region Dismiss Keyboard Property

		private static void TryDismissKeyboard(TextBox textBox)
		{
			if (textBox == null)
			{
				return;
			}

			if (GetDismissKeyboardOnEnter(textBox))
			{
#if __ANDROID__
				if (textBox == null)
				{
					return;
				}

				var inputManager = Uno.UI.ContextHelper.Current.GetSystemService(Android.Content.Context.InputMethodService)
					as Android.Views.InputMethods.InputMethodManager;

				inputManager?.HideSoftInputFromWindow(
					textBox.WindowToken,
					Android.Views.InputMethods.HideSoftInputFlags.None
				);
#else
				return;
#endif
			}
		}
	}
}
#endif
