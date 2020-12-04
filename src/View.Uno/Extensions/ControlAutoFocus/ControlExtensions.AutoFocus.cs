#if WINDOWS_UWP || HAS_WINUI || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#if HAS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Chinook.View.Extensions
{
	public partial class ControlExtensions
	{
		/// <summary>
		/// Get value of IsAutofocusingOnce
		/// </summary>
		/// <param name="obj">Control</param>
		/// <returns>Value of IsAutofocusingOnce</returns>
		public static bool GetIsAutofocusingOnce(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsAutofocusingOnceProperty);
		}

		/// <summary>
		/// Set value of IsAutofocusingOnce
		/// </summary>
		/// <param name="obj">Control</param>
		/// <param name="value">New value of IsAutofocusingOnce</param>
		public static void SetIsAutofocusingOnce(DependencyObject obj, bool value)
		{
			obj.SetValue(IsAutofocusingOnceProperty, value);
		}

		/// <summary>
		/// Property for IsAutofocusingOnce, wich only triggers the first time the control is displayed 
		/// </summary>
		public static readonly DependencyProperty IsAutofocusingOnceProperty =
			DependencyProperty.RegisterAttached("IsAutofocusingOnce", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false, IsAutofocusingOnceChanged));

		/// <summary>
		/// Event raised when IsAutofocusingOnce is changed. It only triggers once
		/// </summary>
		/// <param name="d">Control</param>
		/// <param name="e">Event arguments</param>
		private static void IsAutofocusingOnceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool isAutoFocusOnceEnabled = (bool)e.NewValue;
			var control = d as Control;

			if (isAutoFocusOnceEnabled)
			{
				// We attempt to gain focus in case textbox has already been loaded
				RequestFocus(control);
				control.Loaded -= OnLoadedOnce;
				control.Loaded += OnLoadedOnce;
			}
			else
			{
				control.Loaded -= OnLoadedOnce; // Unsubscribe if we were, if we were not it won't do anything.
			}
		}

		/// <summary>
		/// Event raised when control is loaded for the very first time
		/// </summary>
		/// <param name="sender">Control</param>
		/// <param name="e">Event Arguments</param>
		private static void OnLoadedOnce(object sender, RoutedEventArgs e)
		{
			var control = sender as Control;

			bool isAutoFocusEnabled = GetIsAutofocusingOnce(control);
			if (isAutoFocusEnabled)
			{
				RequestFocus(control);

				// Unsubscribe to loaded once focused
				control.Loaded -= OnLoadedOnce;
			}
			else
			{
				// If we are loaded but AutoFocus isn't enabled, we unsubscribe.
				control.Loaded -= OnLoadedOnce;
			}
		}

		/// <summary>
		/// Get value of IsAutoFocus
		/// </summary>
		/// <param name="obj">Control</param>
		/// <returns></returns>
		public static bool GetIsAutoFocus(Control obj)
		{
			return (bool)obj.GetValue(IsAutoFocusProperty);
		}

		/// <summary>
		/// Set value of IsAutoFocus
		/// </summary>
		/// <param name="obj">Control</param>
		/// <param name="value">IsAutoFocus new value</param>
		public static void SetIsAutoFocus(Control obj, bool value)
		{
			obj.SetValue(IsAutoFocusProperty, value);
		}

		/// <summary>
		/// Property for IsAutoFocus, wich determines if the control receive the focus right fater the page is loaded
		/// </summary>
		public static readonly DependencyProperty IsAutoFocusProperty =
			DependencyProperty.RegisterAttached("IsAutoFocus", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false, IsAutoFocusChanged));

		/// <summary>
		/// Event called when IsAutoFocusProperty is changed
		/// </summary>
		/// <param name="d">Control</param>
		/// <param name="eventArgs">Event arguments</param>
		private static void IsAutoFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		{
			bool isAutoFocusEnabled = (bool)eventArgs.NewValue;
			var control = d as Control;

			if (isAutoFocusEnabled)
			{
				// We attempt to gain focus in case textbox has already been loaded
				RequestFocus(control);
				control.Loaded -= OnLoaded;
				control.Loaded += OnLoaded;
				control.DataContextChanged -= OnDataContextChanged;
				control.DataContextChanged += OnDataContextChanged;
			}
			else
			{
				control.Loaded -= OnLoaded; // Unsubscribe if we were, if we were not it won't do anything.
				control.DataContextChanged -= OnDataContextChanged;
			}
		}

		/// <summary>
		/// Event handler called when control is loaded
		/// </summary>
		/// <param name="sender">Control</param>
		/// <param name="e">Event</param>
		private static void OnLoaded(object sender, RoutedEventArgs e)
		{
			var control = sender as Control;
			ApplyAutoFocus(control);
		}

		/// <summary>
		/// Event handler called when DataContext is changed
		/// </summary>
		/// <param name="sender">Control</param>
		/// <param name="e">Event</param>
		private static void OnDataContextChanged(DependencyObject sender, DataContextChangedEventArgs e)
		{
			var control = sender as Control;
			bool isAutoFocusEnabled = GetIsAutoFocus(control);
			if (isAutoFocusEnabled)
			{
				SetCaret(control);
			}
		}

		/// <summary>
		/// Set focus on the control or unsubscribe
		/// </summary>
		/// <param name="control"></param>
		private static void ApplyAutoFocus(Control control)
		{
			bool isAutoFocusEnabled = GetIsAutoFocus(control);
			if (isAutoFocusEnabled)
			{
				RequestFocus(control);
			}
			else
			{
				// If we are loaded but AutoFocus isn't enabled, we unsubscribe.
				control.Loaded -= OnLoaded;
				control.DataContextChanged -= OnDataContextChanged;
			}
		}

		/// <summary>
		/// Set focus on the control
		/// </summary>
		/// <param name="control"></param>
		private static void RequestFocus(Control control)
		{
			control?.Focus(FocusState.Programmatic);
			SetCaret(control);
		}

		private static void SetCaret(Control control)
		{
			//Making sure the caret position is at the end of the TextBox when focusing for easier editing by the user
			if (control is TextBox)
			{
				var textBox = control as TextBox;

				if (textBox != null && textBox.Text != null)
				{
					textBox.SelectionStart = textBox.Text.Length;
				}
			}
		}
	} 
}
#endif
