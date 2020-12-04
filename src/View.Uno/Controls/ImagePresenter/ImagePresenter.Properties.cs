#if WINDOWS_UWP || HAS_WINUI || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
#if HAS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
#endif

namespace Chinook.View.Controls
{
	public partial class ImagePresenter : Control
	{
#region ImageSource PROPERTY

		public ImageSource ImageSource
		{
			get { return (ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImagePresenter), new PropertyMetadata(default(ImageSource), (s, e) => (s as ImagePresenter)?.OnImageSourceChanged(s, e)));

#endregion

#region ErrorImageSource PROPERTY

		public ImageSource ErrorImageSource
		{
			get { return (ImageSource)GetValue(ErrorImageSourceProperty); }
			set { SetValue(ErrorImageSourceProperty, value); }
		}

		public static readonly DependencyProperty ErrorImageSourceProperty =
			DependencyProperty.Register("ErrorImageSource", typeof(ImageSource), typeof(ImagePresenter), new PropertyMetadata(default(ImageSource), (s, e) => (s as ImagePresenter)?.OnImageSourceChanged(s, e)));

#endregion

#region Stretch PROPERTY

		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImagePresenter), new PropertyMetadata(default(Stretch)));

#endregion

#region IsCircle PROPERTY

		public bool IsCircle
		{
			get { return (bool)GetValue(IsCircleProperty); }
			set { SetValue(IsCircleProperty, value); }
		}

		public static readonly DependencyProperty IsCircleProperty =
			DependencyProperty.Register("IsCircle", typeof(bool), typeof(ImagePresenter), new PropertyMetadata(false));

#endregion

#region PlaceholderContent PROPERTY 

		public object PlaceholderContent
		{
			get { return (object)GetValue(PlaceholderContentProperty); }
			set { SetValue(PlaceholderContentProperty, value); }
		}

		public static readonly DependencyProperty PlaceholderContentProperty =
			DependencyProperty.Register("PlaceholderContent", typeof(object), typeof(ImagePresenter), new PropertyMetadata(default(object)));

#endregion

#region PlaceholderContentTemplate PROPERTY 

		public DataTemplate PlaceholderContentTemplate
		{
			get { return (DataTemplate)GetValue(PlaceholderContentTemplateProperty); }
			set { SetValue(PlaceholderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty PlaceholderContentTemplateProperty =
			DependencyProperty.Register("PlaceholderContentTemplate", typeof(DataTemplate), typeof(ImagePresenter), new PropertyMetadata(default(ContentPresenter)));

#endregion

#region PlaceholderContentTemplateSelector PROPERTY 

		public DataTemplateSelector PlaceholderContentTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(PlaceholderContentTemplateSelectorProperty); }
			set { SetValue(PlaceholderContentTemplateSelectorProperty, value); }
		}

		public static readonly DependencyProperty PlaceholderContentTemplateSelectorProperty =
			DependencyProperty.Register("PlaceholderContentTemplateSelector", typeof(DataTemplateSelector), typeof(ImagePresenter), new PropertyMetadata(default(DataTemplateSelector)));

#endregion

#region Native animations
		/// <summary>
		/// On Android/iOS, this enables native animations for fade transition from placeholder to image. This is supplied as a workaround 
		/// while DoubleAnimation bugs are fixed (Bug #96756). On Windows, this has no effect.
		/// </summary>
		public bool UseNativeAnimations
		{
			get { return (bool)GetValue(UseNativeAnimationsProperty); }
			set { SetValue(UseNativeAnimationsProperty, value); }
		}

		public static readonly DependencyProperty UseNativeAnimationsProperty =
			DependencyProperty.Register("UseNativeAnimations", typeof(bool), typeof(ImagePresenter), new PropertyMetadata(false));


		/// <summary>
		/// The duration of the opacity animation if <see cref="UseNativeAnimations"/> is set to true.
		/// </summary>
		public double NativeAnimationDuration
		{
			get { return (double)GetValue(NativeAnimationDurationProperty); }
			set { SetValue(NativeAnimationDurationProperty, value); }
		}

		public static readonly DependencyProperty NativeAnimationDurationProperty =
			DependencyProperty.Register("NativeAnimationDuration", typeof(double), typeof(ImagePresenter), new PropertyMetadata(0.5));

#endregion
	}
}
#endif
