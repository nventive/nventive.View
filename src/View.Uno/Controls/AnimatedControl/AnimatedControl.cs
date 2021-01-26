#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uno.Extensions;
using Uno.Logging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Controls
{
	[TemplateVisualState(GroupName = AnimationVisualStateGroup, Name = AnimatingVisualState)]
	[TemplateVisualState(GroupName = AnimationVisualStateGroup, Name = NotAnimatingVisualState)]
	public partial class AnimatedControl : Control
	{
		private const string AnimationVisualStateGroup = "Animation";
		private const string AnimatingVisualState = "Animating";
		private const string NotAnimatingVisualState = "NotAnimating";

		private bool _isReady;

		public AnimatedControl()
		{
			this.DefaultStyleKey = typeof(AnimatedControl);
			Loaded += OnControlLoaded;
			Unloaded += OnControlUnloaded;
		}

		#region IsAnimating DEPENDENCY PROPERTY

		/// <summary>
		/// Sets whether the control is in the animating state.
		/// </summary>
		public bool IsAnimating
		{
			get { return (bool)GetValue(IsAnimatingProperty); }
			set { SetValue(IsAnimatingProperty, value); }
		}

		public static readonly DependencyProperty IsAnimatingProperty =
			DependencyProperty.Register("IsAnimating", typeof(bool), typeof(AnimatedControl), new PropertyMetadata(false, OnIsAnimatingChanged));

		private static void OnIsAnimatingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var self = obj as AnimatedControl;

			if (self != null)
			{
				self.Log().Info("OnIsAnimatingChanged stacktrace output: " + System.Environment.StackTrace);
				self.Log().Info ("OnIsAnimatingChanged, NewValue = " + $"{(bool)args.NewValue}");
				self.GoToState((bool)args.NewValue);
			}
		}

		#endregion

		/// <summary>
		/// Whether the animation should be disabled while the control is unloaded. 
		/// </summary>
		/// <remarks>By default this is set to true, since having animations running indefinitely while not visible is a performance drain. </remarks>
		public bool DisableOnUnload
		{
			get { return (bool)GetValue(DisableOnUnloadProperty); }
			set { SetValue(DisableOnUnloadProperty, value); }
		}

		public static readonly DependencyProperty DisableOnUnloadProperty =
			DependencyProperty.Register("DisableOnUnload", typeof(bool), typeof(AnimatedControl), new PropertyMetadata(true));

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_isReady = true;

			this.Log().Info("OnApplyTemplate, IsAnimating = " + $"{IsAnimating}");
			this.GoToState(IsAnimating);
		}

		private void OnControlLoaded(object sender, RoutedEventArgs e)
		{
			if (DisableOnUnload)
			{
				this.Log().Info("OnControlLoaded, IsAnimating = " + $"{IsAnimating}");
				GoToState(IsAnimating);
			}
		}

		private void OnControlUnloaded(object sender, RoutedEventArgs e)
		{
			if (DisableOnUnload)
			{
				this.Log().Info("OnControlUnLoaded, IsAnimating = " + $"{IsAnimating}");
				GoToState(isAnimating: false);
			}
		}

		private void GoToState(bool isAnimating)
		{
			if (_isReady)
			{
				this.Log().Info("GoToState: State = " + $"{(isAnimating ? AnimatingVisualState : NotAnimatingVisualState)}");
				VisualStateManager.GoToState(this, isAnimating ? AnimatingVisualState : NotAnimatingVisualState, true);
			}
		}
	}
}
#endif
