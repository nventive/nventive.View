#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Text;
using Android.Animation;
using Android.Views;
using Windows.UI.Xaml;

namespace Chinook.View.Controls
{
	public partial class ImagePresenter
	{
		private Animator _placeholderAnimator;
		private Animator _imageAnimator;

		private void AnimateOpacity(FrameworkElement element, double targetOpacity, double duration)
		{
			var view = element as Android.Views.View;
			var animator = ObjectAnimator.OfFloat(view, "alpha", (float)targetOpacity);
			animator.SetDuration((long)(NativeAnimationDuration * 1000));
			animator.Start();

			void unsubscribe()
			{
				animator.AnimationCancel -= cancel;
				AnimationEnd -= end;
			}

			void cancel(object sender, EventArgs e)
			{
				unsubscribe();
			}

			void end(object sender, EventArgs e)
			{
				unsubscribe();
				(element as Android.Views.View).Alpha = (float)targetOpacity;
				element.Opacity = targetOpacity;
			}

			animator.AnimationEnd += end;
			animator.AnimationCancel += cancel;

			SetAnimator(element.Name, animator);
		}

		private void SetAnimator(string elementName, Animator value)
		{
			switch (elementName)
			{
				case PlaceholderPartName:
					_placeholderAnimator = value;
					break;
				case ImageBorderPartName:
				case ImagePartName:
					_imageAnimator = value;
					break;
			}
		}

		private void ResetAnimations()
		{
			_placeholderAnimator?.Cancel();
			_placeholderAnimator = null;
			_imageAnimator?.Cancel();
			_imageAnimator = null;
		}
	}
}
#endif
