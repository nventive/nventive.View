#if __IOS__ || __ANDROID__ || __WASM__
using System;
using System.Linq;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Nventive.View.Controls
{
	/// <summary>
	/// Arranges and rotates child elements to ensure that when they are displayed in Landscape, they are always displayed
	/// horizontally and upright. The content appears rotated when the device is in portrait.
	/// </summary>
	public partial class LandscapeUprightPanel : Panel
	{
		private readonly SimpleOrientationSensor _orientationSensor;
		private bool isUpsideDown = false;

		public LandscapeUprightPanel()
		{
			// Prevents touches from going through panel.
			// Required because elements with RotateTransform don't properly handle touches.
			Background = new SolidColorBrush(Colors.Transparent);

			_orientationSensor = SimpleOrientationSensor.GetDefault();

			if (_orientationSensor != null)
			{
				_orientationSensor.OrientationChanged += (s, e) => InvalidateMeasure();
			}

			Loaded += (s, e) => InvalidateMeasure();
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			var baseVal = base.MeasureOverride(new Size(width: availableSize.Height, height: availableSize.Width));
			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			var child = GetChild();
			var angle = GetIsUpsideDown() ? 270 : 90;

			if (child.RenderTransform is RotateTransform rotateTransform)
			{
				rotateTransform.Angle = angle;
			}
			else
			{
				child.RenderTransform = new RotateTransform
				{
					Angle = angle,
				};
			}
#if !__ANDROID__
			child.RenderTransformOrigin = new Point(0.5, 0.5);
#endif

#if __ANDROID__
			var x = GetIsUpsideDown() ? 0 : ActualWidth;
			var y = GetIsUpsideDown() ? ActualHeight : 0;
#elif __IOS__ || __WASM__
			var delta = (ActualHeight - ActualWidth) / 2.0;
			var x = -delta;
			var y = delta;
#endif
			this.ArrangeElement(child, new Rect(x, y, width: ActualHeight, height: ActualWidth));

			return finalSize;
		}

		private bool GetIsUpsideDown()
		{
			switch (_orientationSensor?.GetCurrentOrientation())
			{
				case SimpleOrientation.Rotated270DegreesCounterclockwise:
					isUpsideDown = true;
					return isUpsideDown;

				case SimpleOrientation.Rotated90DegreesCounterclockwise:
					isUpsideDown = false;
					return isUpsideDown;

				case SimpleOrientation.Facedown:
				case SimpleOrientation.Faceup:
				case SimpleOrientation.NotRotated:
				case SimpleOrientation.Rotated180DegreesCounterclockwise:
				default:
					return isUpsideDown;
			}
		}

		private FrameworkElement GetChild() => Children.Single() as FrameworkElement;
	}
}
#endif
