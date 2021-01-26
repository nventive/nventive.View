#if WINDOWS_UWP
using System;
using System.Net;
using System.Windows;
using Uno.Extensions;
using System.Windows.Input;
using Uno.Logging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Uno.Disposables;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Nventive.View.Extensions
{
	public partial class PasswordBoxBehavior : DependencyObject
	{
		#region Attached property: NextControl
		public static Control GetNextControl(PasswordBox obj)
		{
			return obj.GetValue(NextControlProperty) as Control;
		}

		public static void SetNextControl(PasswordBox obj, Control value)
		{
			obj.SetValue(NextControlProperty, value);
		}

		// Using a DependencyProperty as the backing store for EnterCommand.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty NextControlProperty =
			DependencyProperty.RegisterAttached("NextControl", typeof(Control), typeof(PasswordBoxBehavior),
			new PropertyMetadata(null, new PropertyChangedCallback(OnNextControlChanged)));

		private static void OnNextControlChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var passwordBox = sender as PasswordBox;

			if (passwordBox == null)
				return;

			GetOrCreateBehavior(passwordBox);
		}
		#endregion

		#region Attached property: Password

		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxBehavior), new PropertyMetadata(default(string), OnPasswordChanged));

		private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var newValueAsString = (string)e.NewValue;
			if (newValueAsString != null)
				((PasswordBox)d).Password = newValueAsString;

			//Install the behavior after the initial text update.
			GetOrCreateBehavior(((PasswordBox)d));
		}

		public static string GetPassword(PasswordBox textBox)
		{
			return (string)textBox.GetValue(PasswordProperty);
		}

		public static void SetPassword(PasswordBox textBox, string value)
		{
			textBox.SetValue(PasswordProperty, value);
		}

		#endregion

		#region Attached property: AutoUpdateBindingDelay

		public static TimeSpan GetAutoUpdateBindingDelay(PasswordBox obj)
		{
			return (TimeSpan)obj.GetValue(AutoUpdateBindingDelayProperty);
		}

		public static void SetAutoUpdateBindingDelay(PasswordBox obj, TimeSpan value)
		{
			obj.SetValue(AutoUpdateBindingDelayProperty, value);
		}

		public static readonly DependencyProperty AutoUpdateBindingDelayProperty =
			DependencyProperty.RegisterAttached("AutoUpdateBindingDelay", typeof(TimeSpan), typeof(PasswordBoxBehavior),
			new PropertyMetadata(TimeSpan.FromMilliseconds(250), new PropertyChangedCallback(OnAutoUpdateBindingDelayChanged)));

		private static void OnAutoUpdateBindingDelayChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var textBox = sender as PasswordBox;

			if (textBox == null)
				return;

			var isNew = true;

			var behavior = GetOrCreateBehavior(textBox, out isNew);

			if (!isNew)
			{
				behavior._serialDisposable.Disposable = behavior.Attach();
			}
		}

		#endregion

		#region Attached property: IsAutoLostFocusEnabled

		public static bool GetIsAutoLostFocusEnabled(PasswordBox obj)
		{
			return (bool)obj.GetValue(IsAutoLostFocusEnabledProperty);
		}

		public static void SetIsAutoLostFocusEnabled(PasswordBox obj, bool value)
		{
			obj.SetValue(IsAutoLostFocusEnabledProperty, value);
		}

		public static readonly DependencyProperty IsAutoLostFocusEnabledProperty =
			DependencyProperty.RegisterAttached("IsAutoLostFocusEnabled", typeof(bool), typeof(PasswordBoxBehavior),
			new PropertyMetadata(true, new PropertyChangedCallback(OnIsAutoLostFocusEnabledChanged)));

		private static void OnIsAutoLostFocusEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var textBox = sender as PasswordBox;

			if (textBox == null)
				return;

			GetOrCreateBehavior(textBox);
		}

		#endregion

		#region Attached property: IsClearOnSubmitEnabled

		public static bool GetIsClearOnSubmitEnabled(PasswordBox obj)
		{
			return (bool)obj.GetValue(IsClearOnSubmitEnabledProperty);
		}

		public static void SetIsClearOnSubmitEnabled(PasswordBox obj, bool value)
		{
			obj.SetValue(IsClearOnSubmitEnabledProperty, value);
		}

		public static readonly DependencyProperty IsClearOnSubmitEnabledProperty =
			DependencyProperty.RegisterAttached("IsClearOnSubmitEnabled", typeof(bool), typeof(PasswordBoxBehavior),
			new PropertyMetadata(default(bool), new PropertyChangedCallback(OnIsClearOnSubmitEnabledChanged)));

		private static void OnIsClearOnSubmitEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var textBox = sender as PasswordBox;

			if (textBox == null)
				return;

			GetOrCreateBehavior(textBox);
		}

		#endregion

		#region Attached property: Behavior (private)

		private static PasswordBoxBehavior GetOrCreateBehavior(PasswordBox obj)
		{
			bool newlyCreated;

			return GetOrCreateBehavior(obj, out newlyCreated);
		}

		private static PasswordBoxBehavior GetOrCreateBehavior(PasswordBox obj, out bool newlyCreated)
		{
			var behavior = (PasswordBoxBehavior)obj.GetValue(BehaviorProperty);

			newlyCreated = false;
			if (behavior == null)
			{
				behavior = new PasswordBoxBehavior();
				behavior.AssociatedObject = obj;
				obj.SetValue(BehaviorProperty, behavior);

				behavior._serialDisposable.Disposable = behavior.Attach();

				newlyCreated = true;
			}

			return behavior;
		}

		private static readonly DependencyProperty BehaviorProperty =
			DependencyProperty.RegisterAttached("Behavior", typeof(PasswordBoxBehavior), typeof(PasswordBoxBehavior), new PropertyMetadata(null));

		#endregion

		private IDisposable Attach()
		{
			var subscriptions = new CompositeDisposable(2);

			Observable.FromEventPattern<KeyEventHandler, KeyRoutedEventArgs>(
				h => h.Invoke,
				h => AssociatedObject.KeyUp += h,
				h => AssociatedObject.KeyUp -= h,
				Scheduler.Immediate)
				.Select(args => args.EventArgs)
				.Where(args => args != null && (args.Key == Windows.System.VirtualKey.Enter))
				.Subscribe(
					args =>
					{
						ExecuteCommand();
						FocusNextControl();
					},
					ex => this.Log().Error("Exception", ex))
				.DisposeWith(subscriptions);

			Observable
				.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
					h => h.Invoke,
					h => AssociatedObject.PasswordChanged += h,
					h => AssociatedObject.PasswordChanged -= h,
					Scheduler.Immediate)
				.Throttle(AutoUpdateBindingDelay, CoreDispatcherScheduler.Current)
				.Subscribe(
					_ => UpdateBinding(),
					ex => this.Log().Error("Exception", ex))
				.DisposeWith(subscriptions);

			return subscriptions;
		}

		private void UpdateBinding()
		{
			AssociatedObject.SetValue(PasswordProperty, AssociatedObject.Password);
		}

		private void ExecuteCommand()
		{
			var command = GetEnterCommand(AssociatedObject);
			var text = AssociatedObject.Password;

			if (command != null)
			{
				UpdateBinding();

				if (command.CanExecute(text))
				{
					command.Execute(text);

					if (IsClearOnSubmitEnabled)
					{
						AssociatedObject.Password = String.Empty;
					}

					if (IsAutoLostFocusEnabled)
					{
						AssociatedObject.IsEnabled = false;
						AssociatedObject.IsEnabled = true; //Yes this is weird. It's so the textbox loses focus. It also moves the focus to the next control.
					}
				}
			}
			else if (IsAutoLostFocusEnabled)
			{
				AssociatedObject.IsEnabled = false;
				AssociatedObject.IsEnabled = true; //Yes this is weird. It's so the textbox loses focus. It also moves the focus to the next control.
			}
		}

		private void FocusNextControl()
		{
			var element = GetNextControl(AssociatedObject);

			if (element != null)
			{
				var scrollViewer = FindParentRecursive(AssociatedObject.Parent, typeof(ScrollViewer), 10) as ScrollViewer;
				if (scrollViewer != null)
				{
					scrollViewer.ScrollIntoView(element);
				}
				element.Focus(FocusState.Keyboard);
			}
		}

		private static DependencyObject FindParentRecursive(DependencyObject parent, Type targetType, int maxRecursiveCount)
		{
			if (parent == null || maxRecursiveCount == 0)
			{
				return null;
			}

			if (parent.GetType() == targetType)
			{
				return parent;
			}
			return FindParentRecursive(VisualTreeHelper.GetParent(parent), targetType, maxRecursiveCount - 1);
		}

		internal static void Attach(PasswordBox passwordBox)
		{
			GetOrCreateBehavior(passwordBox);
		}

		private PasswordBox AssociatedObject { get; set; }

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

		private TimeSpan AutoUpdateBindingDelay
		{
			get
			{
				return GetAutoUpdateBindingDelay(AssociatedObject);
			}
		}

		private SerialDisposable _serialDisposable = new SerialDisposable();

	}
}
#endif
