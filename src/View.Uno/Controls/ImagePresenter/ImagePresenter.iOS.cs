#if __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Windows.UI.Xaml;

namespace Chinook.View.Controls
{
	public partial class ImagePresenter
	{
		private void AnimateOpacity(FrameworkElement view, double targetOpacity, double duration)
		{
			void animation()
			{
				(view as UIView).Alpha = (nfloat)targetOpacity;
			}

			void completion()
			{
				view.Opacity = targetOpacity;
			}

			UIView.Animate(NativeAnimationDuration, 0, UIViewAnimationOptions.BeginFromCurrentState, animation, completion);
		}

		private void ResetAnimations()
		{
			if (_placeholderPresenter != null)
			{
				_placeholderPresenter.Layer.RemoveAllAnimations();
			}

			if (_imageBorder != null)
			{
				_placeholderPresenter.Layer.RemoveAllAnimations();
			}

			if (_image != null)
			{
				_placeholderPresenter.Layer.RemoveAllAnimations();
			}
		}
	}
}
#endif
