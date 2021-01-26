#if __ANDROID__ || __IOS__
using System;
using System.Windows.Input;
using Uno.Logging;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
#if __ANDROID__ || __IOS__
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.System;
#elif IS_UNIT_TESTS
using Windows.UI.Xaml.Input;
using System.Windows.Controls;
using System.Windows;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.System;
#endif
namespace Nventive.View.Extensions
{
	public class PasswordEnterCommand
	{
		private static readonly Lazy<ILogger> _log = new Lazy<ILogger>(() => typeof(PasswordEnterCommand).Log());

		public static ICommand GetEnterCommand(PasswordBox obj)
		{
			return (ICommand)obj.GetValue(EnterCommandProperty);
		}

		public static void SetEnterCommand(PasswordBox obj, ICommand value)
		{
			obj.SetValue(EnterCommandProperty, value);
		}

		// Using a DependencyProperty as the backing store for EnterCommand.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EnterCommandProperty =
			DependencyProperty.RegisterAttached("EnterCommand", typeof(ICommand), typeof(PasswordEnterCommand), new PropertyMetadata(null, EnterCommandChanged));

		private static void EnterCommandChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var box = d as PasswordBox;
			if (box != null)
			{
				Observable
					.FromEventPattern<KeyEventHandler, object, KeyRoutedEventArgs>(
						h => box.KeyUp += h,
						h => box.KeyUp -= h,
						Scheduler.Immediate)
#pragma warning disable Uno0001 // Uno type or member is not implemented
					.Where(_ => _.EventArgs.Key == VirtualKey.Enter)
#pragma warning restore Uno0001 // Uno type or member is not implemented
					.Subscribe(
						_ => box_KeyUp(_.Sender, _.EventArgs),
						err =>
						{
							var log = _log.Value;
							if (log.IsEnabled(LogLevel.Debug))
							{
								log.DebugFormat("{0}: PasswordEnterCommand KeyUp EnterCommand error: {1} ; {2}", box.Name, err.Message,
									err.StackTrace);
							}
						});
			}
		}

		private static void box_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			var box = sender as PasswordBox;
#pragma warning disable Uno0001 // Uno type or member is not implemented
			if (e.Key == VirtualKey.Enter)
#pragma warning restore Uno0001 // Uno type or member is not implemented
			{
				if (box != null)
				{
					var cmd = GetEnterCommand(box);
					const object param = null;
					if (cmd.CanExecute(param))
					{
						cmd.Execute(param);
						e.Handled = true;
					}
				}
			}
		}
	}
}
#endif
