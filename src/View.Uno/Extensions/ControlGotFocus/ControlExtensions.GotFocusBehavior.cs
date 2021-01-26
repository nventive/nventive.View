#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Extensions
{
	public partial class ControlExtensions
	{
		public static ICommand GetGotFocusCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(GotFocusCommandProperty);
		}

		public static void SetGotFocusCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(GotFocusCommandProperty, value);
		}

		public static object GetGotFocusCommandParameter(DependencyObject obj)
		{
			return obj.GetValue(GotFocusCommandParameterProperty);
		}

		public static void SetGotFocusCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(GotFocusCommandParameterProperty, value);
		}

		public static readonly DependencyProperty GotFocusCommandProperty =
			DependencyProperty.RegisterAttached("GotFocusCommand", typeof(ICommand), typeof(ControlExtensions), new PropertyMetadata(null, OnGotFocusCommandChanged));

		/// <summary>
		/// Value which can be used to specify a parameter to use alongside Command when executing.
		/// </summary>
		public static readonly DependencyProperty GotFocusCommandParameterProperty =
			DependencyProperty.RegisterAttached("GotFocusCommandParameter", typeof(object), typeof(ControlExtensions), new PropertyMetadata(null));

		private static void OnGotFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var sender = (Control)d;

			if (sender != null)
			{
				sender.GotFocus -= OnGotFocus;

				if (GetGotFocusCommand(sender) != null)
				{
					sender.GotFocus += OnGotFocus;
				}
			}
		}

		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			var command = GetGotFocusCommand((DependencyObject)sender);
			var commandParameter = GetGotFocusCommandParameter((DependencyObject)sender);

			if (command != null && command.CanExecute(commandParameter))
			{
				command.Execute(commandParameter);
			}
		}
	}
}
#endif
