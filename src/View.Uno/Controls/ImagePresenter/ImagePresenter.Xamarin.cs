#if __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Controls
{
	public partial class ImagePresenter
	{
		partial void StartNativeAnimation()
		{
			if (_placeholderPresenter != null)
			{
				AnimateOpacity(_placeholderPresenter, 0, NativeAnimationDuration);
			}

			if (_imageBorder != null)
			{
				AnimateOpacity(_imageBorder, 1, NativeAnimationDuration);
			}

			if (_image != null)
			{
				AnimateOpacity(_image, 1, NativeAnimationDuration);
			}
		}

		partial void ResetNativeAnimation()
		{
			ResetAnimations();

			if (_placeholderPresenter != null)
			{
				_placeholderPresenter.Alpha = 1;
				_placeholderPresenter.Opacity = 1;
			}

			if (_imageBorder != null)
			{
				_imageBorder.Alpha = 0;
				_imageBorder.Opacity = 0;
			}

			if (_image != null)
			{
				_image.Alpha = 0;
				_image.Opacity = 0;
			}
		}
	}
}
#endif
