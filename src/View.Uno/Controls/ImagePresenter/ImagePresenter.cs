#if WINDOWS_UWP || HAS_WINUI || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using Uno.Extensions;
using Uno.Logging;
#if HAS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
#endif

namespace Chinook.View.Controls
{
	[TemplatePart(Name = GridPartName, Type = typeof(Grid))]
	[TemplatePart(Name = ImageBorderPartName, Type = typeof(Border))]
	[TemplatePart(Name = ImageBrushPartName, Type = typeof(ImageBrush))]
	[TemplatePart(Name = ImagePartName, Type = typeof(Image))]
	[TemplatePart(Name = PlaceholderPartName, Type = typeof(FrameworkElement))]
	[TemplateVisualState(GroupName = ImageStates.GroupName, Name = ImageStates.Placeholder)]
	[TemplateVisualState(GroupName = ImageStates.GroupName, Name = ImageStates.Image)]
	public partial class ImagePresenter : Control
	{
		private const string GridPartName = "PART_Grid";
		private const string ImageBorderPartName = "PART_ImageBorder";
		private const string ImageBrushPartName = "PART_ImageBrush";
		private const string ImagePartName = "PART_Image";
		private const string PlaceholderPartName = "PART_PlaceholderContentPresenter";

		private class ImageStates
		{
			public const string GroupName = "ImageStates";

			public const string Placeholder = "Placeholder";
			public const string Image = "Image";
		}

		private Grid _grid;
		private Border _imageBorder;
		private ImageBrush _imageBrush;
		private Image _image;
		private FrameworkElement _placeholderPresenter;

		/// <summary>
		/// The ImagePresenter is a control that displays an image (if there is one) or a PlaceholderContentTemplate if there is no image.
		/// If the image is failing you have the additionnal ErrorImageSource property that you can set depending on the desire end result.
		/// The control is stretchable if the width and height are not set and will stay a circle if the IsCircle property is set to true (default is false).
		/// 
		/// LIMITATIONS:
		/// -For a square, the Height and Width must be set.
		/// -For an ellipse (and not a perfect circle) IsCircle must be set to false, and Height and Width must be set, as well as CornerRadius.
		/// </summary>
		public ImagePresenter()
		{
			DefaultStyleKey = typeof(ImagePresenter);

			Loaded += OnLoaded;
			Unloaded += OnUnloaded;

			VisualStateManager.GoToState(this, ImageStates.Placeholder, false);
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (UseNativeAnimations)
			{
				ResetNativeAnimation();
			}

			_grid = this.GetTemplateChild(GridPartName) as Grid;
			_imageBorder = this.GetTemplateChild(ImageBorderPartName) as Border;
			_imageBrush = this.GetTemplateChild(ImageBrushPartName) as ImageBrush;

			if (_imageBrush == null)
			{
				// Uno doesn't currently support GetTemplateChild for ImageBrush.
				// As a workaround, we try to get PART_ImageBrush from PART_ImageBorder.
				_imageBrush = _imageBorder?.Background as ImageBrush;
			}

			_image = this.GetTemplateChild(ImagePartName) as Image;
			_placeholderPresenter = this.GetTemplateChild(PlaceholderPartName) as FrameworkElement;

			UpdateInnerImageSource();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			SizeChanged += OnSizeChanged;
			SetCornerRadius();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			SizeChanged -= OnSizeChanged;
		}

		private void OnImageSourceChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{

			var innerSource = (_imageBrush?.ImageSource ?? _image?.Source) as BitmapImage;
			var bitmapImage = this.ImageSource as BitmapImage;
			if (bitmapImage?.UriSource == innerSource?.UriSource)
			{
				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					this.Log().Debug($"{nameof(ImagePresenter)} has been set to the same source {bitmapImage}, will not be reapplied.");
				}
				return;
			}

			VisualStateManager.GoToState(this, ImageStates.Placeholder, true);

			if (UseNativeAnimations)
			{
				// This is Xamarin-only functionality supplied as a workaround for unreliability of DoubleAnimation.
				ResetNativeAnimation();
			}

			UpdateInnerImageSource();
		}

		partial void ResetNativeAnimation();

		private void UpdateInnerImageSource()
		{
			UnsubscribeHandlers();

			if (ImageSource != null)
			{
				// Callbacks will only be called if ImageSource is non-null
				SubscribeHandlers();

				if (_imageBrush != null)
				{
					_imageBrush.ImageSource = ImageSource;
				}

				if (_image != null)
				{
					_image.Source = ImageSource;
				}
			}
			else if (ErrorImageSource != null)
			{
				// Callbacks will only be called if ErrorImageSource is non-null
				SubscribeHandlers();

				if (_imageBrush != null)
				{
					_imageBrush.ImageSource = ErrorImageSource;
				}

				if (_image != null)
				{
					_image.Source = ErrorImageSource;
				}
			}
			else //Null ImageSource and null ErrorImageSource
			{
				if (_imageBrush != null)
				{
					_imageBrush.ImageSource = null;
				}

				if (_image != null)
				{
					_image.Source = null;
				}
			}
		}

		private void OnImageOpened(object sender, RoutedEventArgs e)
		{
			UnsubscribeHandlers();
			VisualStateManager.GoToState(this, ImageStates.Image, true);

			if (UseNativeAnimations)
			{
				// This is Xamarin-only functionality supplied as a workaround for unreliability of DoubleAnimation.
				StartNativeAnimation();
			}
		}

		partial void StartNativeAnimation();

		private void OnImageFailed(object sender, RoutedEventArgs e)
		{
			UnsubscribeHandlers();

			if (ErrorImageSource != null)
			{
				if (_imageBrush != null)
				{
					_imageBrush.ImageSource = ErrorImageSource;
				}

				if (_image != null)
				{
					_image.Source = ErrorImageSource;
				}

				VisualStateManager.GoToState(this, ImageStates.Image, true);

				if (UseNativeAnimations)
				{
					// This is Xamarin-only functionality supplied as a workaround for unreliability of DoubleAnimation.
					StartNativeAnimation();
				}
			}
		}

		private void UnsubscribeHandlers()
		{
			if (_imageBrush != null)
			{
				_imageBrush.ImageFailed -= OnImageFailed;
				_imageBrush.ImageOpened -= OnImageOpened;
			}

			if (_image != null)
			{
				_image.ImageFailed -= OnImageFailed;
				_image.ImageOpened -= OnImageOpened;
			}
		}

		private void SubscribeHandlers()
		{
			if (_imageBrush != null)
			{
				_imageBrush.ImageOpened += OnImageOpened;
				_imageBrush.ImageFailed += OnImageFailed;
			}

			if (_image != null)
			{
				_image.ImageOpened += OnImageOpened;
				_image.ImageFailed += OnImageFailed;
			}
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			SetCornerRadius();
		}

		private void SetCornerRadius()
		{
			if (IsCircle)
			{
				double size = 1;

				//If the width and height of the control are set, it will take the smallest of the two
				//and create a circle with it. If only one is set, it will take this one.
				if (!double.IsNaN(this.Width) || !double.IsNaN(this.Height))
				{
					//The circle could become an oval 
					//if the height and width are not the same. This prevents it.
					if (this.Height < this.Width || double.IsNaN(this.Width))
					{
						_grid.Width = this.Height;
						_grid.Height = this.Height;
						size = this.Height;
					}
					else
					{
						_grid.Height = this.Width;
						_grid.Width = this.Width;
						size = this.Width;
					}
				}

				//If neither the width or height is set it will take the smallest of the ActualHeight or ActualWidth
				//and create a perfect circle with it.
				else if (this.ActualHeight != 0 && this.ActualWidth != 0)
				{
					//Since the control is stretchable, the circle could become an oval 
					//if the height and width are not the same. This prevents it.
					if (this.ActualHeight < this.ActualWidth)
					{
						_grid.Width = this.ActualHeight;
						_grid.Height = this.ActualHeight;
						size = this.ActualHeight;
					}
					else
					{
						_grid.Height = this.ActualWidth;
						_grid.Width = this.ActualWidth;
						size = this.ActualWidth;
					}
				}

				CornerRadius = new CornerRadius(size / 2);
			}
		}
	}
}
#endif
