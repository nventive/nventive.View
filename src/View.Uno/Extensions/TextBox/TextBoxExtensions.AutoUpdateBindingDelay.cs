#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using Uno.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Extensions
{
	public class TextBoxExtendedProperties
	{
		#region Disposal

		private static TextBoxExtendedProperties GetOrCreateBehavior(DependencyObject obj)
		{
			var attachedBehavior = GetAttachedBehavior(obj);
			if (attachedBehavior == null)
			{
				attachedBehavior = new TextBoxExtendedProperties();
				SetAttachedBehavior(obj, attachedBehavior);
			}
			return attachedBehavior;
		}

		private static TextBoxExtendedProperties GetAttachedBehavior(DependencyObject obj)
		{
			return (TextBoxExtendedProperties)obj.GetValue(AttachedBehaviorProperty);
		}

		private static void SetAttachedBehavior(DependencyObject obj, TextBoxExtendedProperties value)
		{
			obj.SetValue(AttachedBehaviorProperty, value);
		}

		private static readonly DependencyProperty AttachedBehaviorProperty =
			DependencyProperty.RegisterAttached("AttachedBehavior", typeof(TextBoxExtendedProperties), typeof(TextBoxExtendedProperties), new PropertyMetadata(null));

		#endregion

		private static readonly TimeSpan DefaultAutoUpdateBindingDelay = TimeSpan.FromMilliseconds(250);
		
		public static TimeSpan GetAutoUpdateBindingDelay(DependencyObject obj)
		{
			return (TimeSpan)obj.GetValue(AutoUpdateBindingDelayProperty);
		}

		public static void SetAutoUpdateBindingDelay(DependencyObject obj, TimeSpan value)
		{
			obj.SetValue(AutoUpdateBindingDelayProperty, value);
		}

		public static readonly DependencyProperty AutoUpdateBindingDelayProperty =
			DependencyProperty.RegisterAttached("AutoUpdateBindingDelay", typeof(TimeSpan), typeof(TextBoxExtendedProperties), new PropertyMetadata(DefaultAutoUpdateBindingDelay, AutoUpdateBindingDelayChanged));

		private static void AutoUpdateBindingDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			var textBox = d as TextBox;
			if (textBox == null || args.NewValue == null)
			{
				return;
			}
			else
			{
				SubscribeToUpdateBinding(textBox);
			}
		}
		
		private static void SubscribeToUpdateBinding(TextBox textBox)
		{
			var attachedBehavior = GetOrCreateBehavior(textBox);
			if (attachedBehavior != null)
			{
				Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
						h => h.Invoke,
						h => textBox.TextChanged += h,
						h => textBox.TextChanged -= h,
						Scheduler.Immediate)
					.Throttle(GetAutoUpdateBindingDelay(textBox), new MainDispatcherScheduler(textBox.Dispatcher))
					.Do(_ => UpdateBinding(textBox))
					.SubscribeToElement(textBox, _ => { }, _ => { });
			}
		}

		private static void UpdateBinding(TextBox textBox)
		{
			var binding = textBox.GetBindingExpression(TextBox.TextProperty);

			if (binding != null)
			{
#if WINDOWS_UWP
				binding.UpdateSource();
#else
				// Uno does not support the parameterless version
				binding.UpdateSource(textBox.Text);
#endif
			}
		}
	}
}
#endif
