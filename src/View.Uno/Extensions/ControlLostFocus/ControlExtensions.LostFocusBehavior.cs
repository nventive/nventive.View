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
		public static ICommand GetLostFocusCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(LostFocusCommandProperty);
		}

		public static void SetLostFocusCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(LostFocusCommandProperty, value);
		}

		public static object GetLostFocusCommandParameter(DependencyObject obj)
		{
			return obj.GetValue(LostFocusCommandParameterProperty);
		}

		public static void SetLostFocusCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(LostFocusCommandParameterProperty, value);
		}

		public static readonly DependencyProperty LostFocusCommandProperty =
			DependencyProperty.RegisterAttached("LostFocusCommand", typeof(ICommand), typeof(ControlExtensions), new PropertyMetadata(null, OnLostFocusCommandChanged));

		/// <summary>
		/// Value which can be used to specify a parameter to use alongside Command when executing.
		/// </summary>
		public static readonly DependencyProperty LostFocusCommandParameterProperty =
			DependencyProperty.RegisterAttached("LostFocusCommandParameter", typeof(object), typeof(ControlExtensions), new PropertyMetadata(null));

		private static void OnLostFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var sender = (Control)d;

			if (sender != null)
			{
				sender.LostFocus -= OnLostFocus;

				if (GetLostFocusCommand(sender) != null)
				{
					sender.LostFocus += OnLostFocus;
				}
			}
		}

		private static void OnLostFocus(object sender, RoutedEventArgs e)
		{
			var command = GetLostFocusCommand((DependencyObject)sender);
			var commandParameter = GetLostFocusCommandParameter((DependencyObject)sender);

			if (command != null && command.CanExecute(commandParameter))
			{
				command.Execute(commandParameter);
			}
		}
	}
}
#endif
