#if WINDOWS_UWP
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Text;
using System.Globalization;
using Uno.Disposables;

namespace Nventive.View.Extensions
{
	public partial class TextBoxBehavior : DependencyObject
	{
#region AutoInputFormat DEPENDENCY PROPERTY

		public enum AutoInputFormats
		{
			/// <summary>
			/// Default, you can type anything
			/// </summary>
			None,

			/// <summary>
			/// You can only type numbers and one culture dependent digit separator
			/// </summary>
			DecimalNumber
		}

		public static AutoInputFormats GetAutoInputFormat(DependencyObject obj)
		{
			return (AutoInputFormats)obj.GetValue(AutoInputFormatProperty);
		}

		public static void SetAutoInputFormat(DependencyObject obj, AutoInputFormats value)
		{
			obj.SetValue(AutoInputFormatProperty, value);
		}

		public static readonly DependencyProperty AutoInputFormatProperty =
			DependencyProperty.RegisterAttached("AutoInputFormat", typeof(AutoInputFormats), typeof(TextBoxBehavior), new PropertyMetadata(AutoInputFormats.None, OnAutoInputFormatChanged));

		private static void OnAutoInputFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textbox = (TextBox)d;

			textbox.TextChanging -= OnTextChangingForAutoInputFormat;
			textbox.TextChanged -= OnTextChangedForAutoInputFormat;

			var autoInputFormat = (AutoInputFormats)e.NewValue;
			switch (autoInputFormat)
			{
				case AutoInputFormats.DecimalNumber:
					textbox.TextChanging += OnTextChangingForAutoInputFormat;
					textbox.TextChanged += OnTextChangedForAutoInputFormat;
					break;
				default:
					break;
			}
		}

		private static void OnTextChangingForAutoInputFormat(TextBox sender, TextBoxTextChangingEventArgs args)
		{
			var raw = sender.Text;
			var sanitizer = new StringBuilder(raw?.Length ?? 0);
			var hasDecimalSeparator = false;
			var decimalSeparatorSymbol = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			for (int i = 0; i < raw.Length; i++)
			{
				var c = raw[i];
				if (char.IsNumber(c))
				{
					sanitizer.Append(c);
				}
				else if (!hasDecimalSeparator && (c == '.' || c == ',' || c.ToString() == decimalSeparatorSymbol))
				{
					// If you type "." or "," it writes the good one based on the current culture
					if (sanitizer.Length == 0)
					{
						// If you type ".1", it writes "0.1" instead
						sanitizer.Append('0');
					}

					sanitizer.Append(decimalSeparatorSymbol);
					hasDecimalSeparator = true;
				}
			}

			var sanitized = sanitizer.ToString();

			if (raw != sanitized)
			{
				sender.Text = sanitized;
			}
		}

		private static void OnTextChangedForAutoInputFormat(object sender, TextChangedEventArgs e)
		{
			var textBox = (TextBox)sender;

			// Set cursor position at the end.
			// This is required because changing the Text in the TextChanging event handler doesn't change the cursor position.
			textBox.SelectionStart = Math.Max(0, textBox.Text?.Length ?? 0);
			textBox.SelectionLength = 0;
		}
#endregion

#region Attached property: NextControlScrollViewer
		public static ScrollViewer GetNextControlScrollViewer(TextBox obj)
		{
			return obj.GetValue(NextControlScrollViewerProperty) as ScrollViewer;
		}

		/// <summary>
		/// Set the scroll viewer which contains the <see cref="NextControl"/> and used to ensure that <see cref="NextControl"/> is visible 
		/// (<see cref="NextControlScrollViewer"/>.<seealso cref="ScrollViewerExtensions.ScrollIntoView(ScrollViewer, FrameworkElement, double, double, TimeSpan)"/>(<see cref="NextControl"/>)). 
		/// If null, and <see cref="NextControl"/> not null, then ScrollViewer will be search in visual tree hierachy.
		/// </summary>
		public static void SetNextControlScrollViewer(TextBox obj, ScrollViewer value)
		{
			obj.SetValue(NextControlScrollViewerProperty, value);
		}

		public static readonly DependencyProperty NextControlScrollViewerProperty =
			DependencyProperty.RegisterAttached("NextControlScrollViewer", typeof(ScrollViewer), typeof(TextBoxBehavior),
			new PropertyMetadata(null));
#endregion

#region Attached property: IsAutoLostFocusEnabled

		public static bool GetIsAutoLostFocusEnabled(TextBox obj)
		{
			return (bool)obj.GetValue(IsAutoLostFocusEnabledProperty);
		}

		public static void SetIsAutoLostFocusEnabled(TextBox obj, bool value)
		{
			obj.SetValue(IsAutoLostFocusEnabledProperty, value);
		}

		public static readonly DependencyProperty IsAutoLostFocusEnabledProperty =
			DependencyProperty.RegisterAttached("IsAutoLostFocusEnabled", typeof(bool), typeof(TextBoxBehavior),
			new PropertyMetadata(true));

#endregion

#region Attached property: IsClearOnSubmitEnabled

		public static bool GetIsClearOnSubmitEnabled(TextBox obj)
		{
			return (bool)obj.GetValue(IsClearOnSubmitEnabledProperty);
		}

		public static void SetIsClearOnSubmitEnabled(TextBox obj, bool value)
		{
			obj.SetValue(IsClearOnSubmitEnabledProperty, value);
		}

		public static readonly DependencyProperty IsClearOnSubmitEnabledProperty =
			DependencyProperty.RegisterAttached("IsClearOnSubmitEnabled", typeof(bool), typeof(TextBoxBehavior),
			new PropertyMetadata(default(bool)));
#endregion

		/// <summary>
		/// Dismisses the keyboard
		/// </summary>
		/// <param name="textBox">Target TextBox</param>
		private static void TryDismissKeyboard(TextBox textBox)
		{
			if (textBox == null || GetDismissKeyboardOnEnter(textBox))
			{
				return;
			}
		}

		private static void FocusNextControl(TextBox textBox)
		{
			var element = GetNextControl(textBox);

			if (element != null)
			{
				var scrollViewer = GetNextControlScrollViewer(textBox) ?? element.FindFirstParent<ScrollViewer>();
				if (scrollViewer != null)
				{
					scrollViewer.ScrollIntoView(element);
				}

				element.Focus(FocusState.Keyboard);
			}
		}

		private static void TryExecuteEnterCommand(object sender, KeyRoutedEventArgs eventArgs)
		{
			var textBox = (TextBox)sender;

			var command = GetEnterCommand(textBox);
			var text = textBox.Text;

			if (command != null)
			{
				if (command.CanExecute(text))
				{
					command.Execute(text);

					if (GetIsClearOnSubmitEnabled(textBox))
					{
						textBox.Text = String.Empty;
					}

					if (GetIsAutoLostFocusEnabled(textBox))
					{
						textBox.IsEnabled = false;
						textBox.IsEnabled = true; //Yes this is weird. It's so the textbox loses focus. It also moves the focus to the next control.
					}
				}
			}
			else if (GetIsAutoLostFocusEnabled(textBox))
			{
				textBox.IsEnabled = false;
				textBox.IsEnabled = true; //Yes this is weird. It's so the textbox loses focus. It also moves the focus to the next control.
			}
		}

		private TextBox AssociatedObject { get; set; }

		private bool IsAutoLostFocusEnabled
		{
			get
			{
				return GetIsAutoLostFocusEnabled(AssociatedObject);
			}
		}

		private bool IsClearOnSubmitEnabled
		{
			get
			{
				return GetIsClearOnSubmitEnabled(AssociatedObject);
			}
		}

		private SerialDisposable _serialDisposable = new SerialDisposable();
	}
}
#endif
