#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Nventive.View.Extensions
{
	public partial class InputPaneExtensions
	{
		#region Dependency Property: PanIntoView
		/// <summary>
		/// Get value of PanIntoView
		/// </summary>
		/// <param name="obj">FrameworkElement</param>
		/// <returns>Value of PanIntoView</returns>
		public static bool GetPanIntoView(DependencyObject obj)
		{
			return (bool)obj.GetValue(PanIntoViewProperty);
		}

		/// <summary>
		/// Set value of PanIntoView
		/// </summary>
		/// <param name="obj">FrameworkElement</param>
		/// <param name="value">New value of PanIntoView</param>
		public static void SetPanIntoView(DependencyObject obj, bool value)
		{
			obj.SetValue(PanIntoViewProperty, value);
		}

		/// <summary>
		/// Property for PanIntoView, which only triggers the first time the FrameworkElement is displayed 
		/// </summary>
		public static readonly DependencyProperty PanIntoViewProperty =
			DependencyProperty.RegisterAttached("PanIntoView", typeof(bool), typeof(InputPaneExtensions), new PropertyMetadata(false, PanIntoViewChanged));
		#endregion

		#region Dependency Property: PreviousMargin
		/// <summary>
		/// Get value of PreviousMargin
		/// </summary>
		/// <param name="obj">FrameworkElement</param>
		/// <returns>Value of PreviousMargin</returns>
		private static Thickness GetPreviousMargin(DependencyObject obj)
		{
			return (Thickness)obj.GetValue(PreviousMarginProperty);
		}

		/// <summary>
		/// Set value of PreviousMargin
		/// </summary>
		/// <param name="obj">FrameworkElement</param>
		/// <param name="value">New value of Previous Value</param>
		private static void SetPreviousMargin(DependencyObject obj, Thickness value)
		{
			obj.SetValue(PreviousMarginProperty, value);
		}

		/// <summary>
		/// Property to keep the PreviousMargin for use in Android
		/// https://nventive.visualstudio.com/Umbrella/_workitems/edit/164383
		/// </summary>
		private static readonly DependencyProperty PreviousMarginProperty =
			DependencyProperty.RegisterAttached("PreviousMargin", typeof(Thickness), typeof(InputPaneExtensions), new PropertyMetadata(default(Thickness)));
		#endregion

		/// <summary>
		/// Event raised when PanIntoViewChanged is changed. It only triggers once
		/// </summary>
		/// <param name="d">FrameworkElement</param>
		/// <param name="e">Event arguments</param>
		private static void PanIntoViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as FrameworkElement;

#if __ANDROID__
			SetPreviousMargin(element, element.Margin);
#endif

			InputPane.GetForCurrentView().Showing += (s, args) => OnKeyboardShowing(element, args);
			InputPane.GetForCurrentView().Hiding += async (s, args) => await OnKeyboardHiding(CancellationToken.None, element, args);
		}

		/// <summary>
		/// Event raised when the keyboard is showing
		/// </summary>
		/// <param name="target">FrameworkElement</param>
		/// <param name="args">InputPane Event Arguments</param>
		private static void OnKeyboardShowing(FrameworkElement target, InputPaneVisibilityEventArgs args)
		{
			var translate = target.RenderTransform as TranslateTransform ?? new TranslateTransform();
			target.RenderTransform = translate;

			var visibleBounds = ApplicationView.GetForCurrentView().VisibleBounds;
			var bounds = Window.Current.Bounds;

			var translateTo = -args.OccludedRect.Height + bounds.Bottom - visibleBounds.Bottom;

#if __ANDROID__
			// Adding Margin to the Bottom to show the Message TextField because of clipboard issue : 
			// https://nventive.visualstudio.com/Umbrella/_workitems/edit/164383

			var previousMargin = GetPreviousMargin(target);

			target.Margin = new Thickness(previousMargin.Left, previousMargin.Top, previousMargin.Right, previousMargin.Bottom - translateTo);
#else
			AnimateInputPane(translate, from: translate.Y, to: translateTo);
#endif
		}

		/// <summary>
		/// Event raised when the keyboard is hidding
		/// </summary>
		/// <param name="ct">CancellationToken</param>
		/// <param name="target">FrameworkElement</param>
		/// <param name="args">InputPane Event Arguments</param>
		private static async Task OnKeyboardHiding(CancellationToken ct, FrameworkElement target, InputPaneVisibilityEventArgs args)
		{
			var translate = target.RenderTransform as TranslateTransform ?? new TranslateTransform();
			target.RenderTransform = translate;

#if __ANDROID__
			// Using the Previous Margin to hide the Message TextField because of clipboard issue :
			// https://nventive.visualstudio.com/Umbrella/_workitems/edit/164383

			target.Margin = GetPreviousMargin(target);
#else
			// Give time to events to complete
			await Task.Delay(100, ct);

			var translateTo = args.OccludedRect.Height;

			AnimateInputPane(translate, from: translate.Y, to: translateTo);

			// Give more time to the InputPane to hide.
			await Task.Delay(300, ct);
#endif
		}

		private static void AnimateInputPane(TranslateTransform translate, double from, double to)
		{
			var anim = new DoubleAnimation()
			{
				From = from,
				To = to,
				Duration = new Duration(TimeSpan.FromMilliseconds(500)),
				EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
			};

			var storyboard = new Storyboard();

			Storyboard.SetTarget(anim, translate);
			Storyboard.SetTargetProperty(anim, nameof(translate.Y));

			storyboard.Children.Add(anim);
			storyboard.Begin();
		}
	}
}
#endif
