#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Uno.Extensions;
using Uno.Logging;
using Uno.Disposables;
#if WINDOWS_UWP
using IFrameworkElement = Windows.UI.Xaml.FrameworkElement;
using View = Windows.UI.Xaml.FrameworkElement;
using Size = Windows.Foundation.Size;
#elif __ANDROID__ || __IOS__ || __WASM__
using IFrameworkElement = Windows.UI.Xaml.FrameworkElement;
#endif
#if __IOS__
using Foundation;
using View = UIKit.UIView;
#elif __ANDROID__
using View = Android.Views.View;
#endif

namespace Nventive.View.Controls
{
	/// <summary>
	/// Represents a scrollable and zoomable area that can contain other visible elements.
	/// </summary>
	/// <remarks>
	///	<para>ZoomControl was designed to be used for image cropping, but should be flexible enough to be used in other scenarios.</para>
	///	<para>It is very similar to a ScrollViewer (which it contains), and can be used the same way.</para>
	///	<para>Unlike ScrollViewer, zooming, horizontal scrolling and vertical scrolling are always enabled.</para>
	/// </remarks>
	[ContentProperty(Name = "Content")]
	[TemplatePart(Name = "ScrollViewer", Type = typeof(ScrollViewer))]
	[TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
	public partial class ZoomControl : ContentControl
	{
		private ScrollViewer _scrollViewer;
		private ContentPresenter _contentPresenter;

		// This is used by OnViewPortChanged to determine whether the ViewPort 
		// was set internally (using UpdateViewPort) or externally.
		private Rect _lastCalculatedViewPort = Rect.Empty;

		private readonly SerialDisposable _callbackSubscriptions = new SerialDisposable();
		private bool _isLoaded;

		public ZoomControl()
		{
			DefaultStyleKey = typeof(ZoomControl);

			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}

		#region ViewPort

		/// <summary>
		/// Gets or sets a value that indicates the area of the Content visible through the viewport. 
		/// </summary>
		/// <remarks>
		///	<para>The values (between 0 and 1) are relative to the size of the Content, where 1 represents the full width/height of the Content.</para>
		///	<para>This property is generally consumed by the view model to be used for image cropping.</para>
		///	<para>Setting the ViewPort is currently not supported and will be ignored.</para>
		/// </remarks>
		public Rect ViewPort
		{
			get { return (Rect)this.GetValue(ViewPortProperty); }
			set { this.SetValue(ViewPortProperty, value); }
		}

		public static readonly DependencyProperty ViewPortProperty =
			DependencyProperty.Register(
				"ViewPort",
				typeof(Rect),
				typeof(ZoomControl),
				new PropertyMetadata(
					Rect.Empty,
					(s, e) => (s as ZoomControl)?.OnViewPortChanged((Rect)e.OldValue, (Rect)e.NewValue)
			));

		private void OnViewPortChanged(Rect oldValue, Rect newValue)
		{
			if (newValue != _lastCalculatedViewPort)
			{
				this.Log().Warn($"ZoomControl.ViewPort was set externally to {newValue}, which is not supported. Reverting back to previous value of {oldValue}");

				// Uno doesn't currently allow a DependencyProperty to be changed from within its PropertyChangedCallback.
				// Scheduling the update bypasses this.

				_ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
				{
					ViewPort = oldValue;
				});
			}
		}

		#endregion

		#region Stretch

		/// <summary>
		///	Gets or sets a value that describes how the Content should be stretched to fill the viewport.
		/// </summary>
		/// <remarks>
		///	<para>This is very similar to Image.Stretch, and can be expected to work the same way.</para>
		///	<list type="bullet">
		///	<listheader>
		///	<description>None</description>
		///	</listheader>
		///	<item>
		///	<description>Preserves the original size of the Content.</description>
		///	</item>
		///	<item>
		///	<description>Doesn't work when the Content is smaller than the viewport.</description>
		///	</item>
		/// </list>
		/// <list type="bullet">
		///	<listheader>
		///	<description>Uniform</description>
		///	</listheader>
		///	<item>
		///	<description>Content fit the viewport (the entire surface of the Content is visible).</description>
		///	</item>
		///	<item>
		///	<description>Doesn't properly align the Content in the viewport.</description>
		///	</item>
		///	<item>
		///	<description>Doesn't properly update the ViewPort property.</description>
		///	</item>
		/// </list>
		/// <list type="bullet">
		///	<listheader>
		///	<description>UniformToFill</description>
		///	</listheader>
		///	<item>
		///	<description>Content fill the viewport (no empty/blank area around Content).</description>
		///	</item>
		///	<item>
		///	<description>Only well-supported value of Stretch.</description>
		///	</item>
		///	<item>
		///	<description>Most commonly used for image cropping.</description>
		///	</item>
		/// </list>
		/// <list type="bullet">
		///	<listheader>
		///	<description>Fill</description>
		///	</listheader>
		///	<item>
		///	<description>Not supported (by design).</description>
		///	</item>
		/// </list>
		/// </remarks>
		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ZoomControl), new PropertyMetadata(Stretch.None, (s, e) => (s as ZoomControl)?.OnStretchChanged((Stretch)e.OldValue, (Stretch)e.NewValue)));

		#endregion

		#region AspectRatio

		/// <summary>
		/// Gets or sets a value that describes the aspect ratio ZoomControl should preserve.
		/// </summary>
		/// <remarks>
		/// <para>The aspect ratio is applied to the ZoomControl, and not to the Content.</para> 
		/// <para>This property is generally used in conjunction with image cropping, with a value of "1,1" (square, 1:1 aspect ratio).</para>
		/// </remarks>
		public Size AspectRatio
		{
			get { return (Size)GetValue(AspectRatioProperty); }
			set { SetValue(AspectRatioProperty, value); }
		}

		public static readonly DependencyProperty AspectRatioProperty =
			DependencyProperty.Register("AspectRatio", typeof(Size), typeof(ZoomControl), new PropertyMetadata(Size.Empty, (s, e) => (s as ZoomControl)?.OnAspectRatioChanged((Size)e.OldValue, (Size)e.NewValue)));

		#endregion

		/// <summary>
		/// Resets the ViewPort to it's initial value (resets scroll offset and re-applies aspect ratio).
		/// </summary>
		public void ResetViewPort()
		{
			_lastCalculatedViewPort = Rect.Empty;
			ViewPort = Rect.Empty;

			ApplyStretch();
		}

		/// <summary>
		/// Gets whether _scrollViewer or _contentPresenter is empty.
		/// </summary>
		private bool IsEmpty => _scrollViewer == null
			|| _contentPresenter == null
			|| _scrollViewer.ActualHeight == 0
			|| _scrollViewer.ActualWidth == 0
			|| _contentPresenter.ActualWidth == 0
			|| _contentPresenter.ActualHeight == 0;

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
			if (_scrollViewer == null)
			{
				throw new InvalidOperationException("The template part ScrollViewer could not be found or is not a ScrollViewer");
			}

			_contentPresenter = GetTemplateChild("ContentPresenter") as ContentPresenter;
			if (_contentPresenter == null)
			{
				throw new InvalidOperationException("The template part ContentPresenter could not be found or is not a ContentPresenter");
			}

			if (_isLoaded)
			{
				_callbackSubscriptions.Disposable = SubscribeToCallbacks();
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = true;
			_callbackSubscriptions.Disposable = SubscribeToCallbacks();

			//this is necessary (size may change after OnApplyTemplate but before OnLoaded)
			ApplyStretch();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = false;
			_callbackSubscriptions.Disposable = null;
		}

		private IDisposable SubscribeToCallbacks()
		{
			// Close over current values in case template changes
			var contentPresenter = _contentPresenter;
			var scrollViewer = _scrollViewer;

			if (contentPresenter != null && scrollViewer != null)
			{
				contentPresenter.SizeChanged += OnSizeChanged;
				scrollViewer.SizeChanged += OnSizeChanged;
				scrollViewer.ViewChanged += OnViewChanged;

				return Disposable.Create(() =>
				{
					contentPresenter.SizeChanged -= OnSizeChanged;
					scrollViewer.SizeChanged -= OnSizeChanged;
					scrollViewer.ViewChanged -= OnViewChanged;
				});
			}
			else
			{
				return null;
			}
		}

		private void OnAspectRatioChanged(Size oldValue, Size newValue) => this.InvalidateMeasure();
		private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e) => UpdateViewPort();
		private void OnSizeChanged(object sender, SizeChangedEventArgs e) => ApplyStretch();
		private void OnStretchChanged(Stretch oldValue, Stretch newValue) => ApplyStretch();

		/// <summary>
		/// Update ViewPort according to _scrollViewer's zoom/offsets.
		/// </summary>
		private void UpdateViewPort()
		{
			if (IsEmpty)
			{
				_lastCalculatedViewPort = Rect.Empty;
			}
			else
			{
				_lastCalculatedViewPort = new Rect
				{
					// Rounding prevents cases where Width/Height goes over 1 (i.e., 1.00000004)
					X = Math.Round(_scrollViewer.HorizontalOffset / _contentPresenter.ActualWidth / _scrollViewer.ZoomFactor, 6),
					Y = Math.Round(_scrollViewer.VerticalOffset / _contentPresenter.ActualHeight / _scrollViewer.ZoomFactor, 6),
					Width = Math.Round(_scrollViewer.ActualWidth / _contentPresenter.ActualWidth / _scrollViewer.ZoomFactor, 6),
					Height = Math.Round(_scrollViewer.ActualHeight / _contentPresenter.ActualHeight / _scrollViewer.ZoomFactor, 6),
				};
			}

			ViewPort = _lastCalculatedViewPort;
		}

		/// <summary>
		/// Update _scrollViewer.ZoomFactor according to Stretch.
		/// </summary>
		private void ApplyStretch()
		{
			if (IsEmpty)
			{
				return;
			}

			var contentAspectRatio = _contentPresenter.ActualHeight / _contentPresenter.ActualWidth;
			var controlAspectRatio = _scrollViewer.ActualHeight / _scrollViewer.ActualWidth;

			double newMinZoomFactor;

			switch (Stretch)
			{
				case Stretch.None:
					newMinZoomFactor = 1d;
					break;
				case Stretch.Uniform:
					newMinZoomFactor = contentAspectRatio > controlAspectRatio
						? _scrollViewer.ActualHeight / _contentPresenter.ActualHeight   // content is taller
						: _scrollViewer.ActualWidth / _contentPresenter.ActualWidth;    // content is wider
					break;
				case Stretch.UniformToFill:
					newMinZoomFactor = contentAspectRatio > controlAspectRatio
						? _scrollViewer.ActualWidth / _contentPresenter.ActualWidth     // content is taller
						: _scrollViewer.ActualHeight / _contentPresenter.ActualHeight;  // content is wider
					break;
				case Stretch.Fill:
				default:
					throw new NotImplementedException($"{nameof(ZoomControl)}.{nameof(Stretch)} doesn't support value {Stretch}.");
			}

#if WINDOWS_UWP
			if (newMinZoomFactor < 0.1)
			{
				this.Log().Warn("ZoomControl couldn't make the content fit the viewport because it's more than 10 times its size. This is due to a limitation with the inner Windows ScrollViewer whose ZoomFactor can't go below 0.1.");
				newMinZoomFactor = 0.1;
			}
#endif

			_scrollViewer.MinZoomFactor = (float)newMinZoomFactor;
			_scrollViewer.ChangeView(0, 0, (float)newMinZoomFactor, disableAnimation: true);

			UpdateViewPort();
		}

		protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
		{
			// Apply AspectRatio
			var newAvailableSize = ApplyAspectRatio(availableSize, AspectRatio);
			base.MeasureOverride(newAvailableSize);
			return newAvailableSize;
		}

		protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
		{
			// Apply AspectRatio
			var newFinalSize = ApplyAspectRatio(finalSize, AspectRatio);
			base.ArrangeOverride(newFinalSize);
			return newFinalSize;
		}

		/// <summary>
		/// Applies an aspectRatio to a size.
		/// </summary>
		/// <param name="size">available size</param>
		/// <param name="aspectRatio">aspect ratio</param>
		/// <returns>largest size that both respects aspectRatio and fits size</returns>
		private static Windows.Foundation.Size ApplyAspectRatio(Windows.Foundation.Size size, Size aspectRatio)
		{
			if (aspectRatio.IsEmpty) // No AspectRatio set
			{
				return size;
			}

			if (size.Width == 0 || size.Height == 0 || aspectRatio.Width == 0 || aspectRatio.Height == 0)
			{
				return default(Size);
			}

			var actualRatio = size.Height / size.Width;
			var requestedRatio = aspectRatio.Height / aspectRatio.Width;

			return actualRatio > requestedRatio
				? new Windows.Foundation.Size(size.Width, size.Width * (float)requestedRatio)      // taller than requested
				: new Windows.Foundation.Size(size.Height / (float)requestedRatio, size.Height);   // wider than requested
		}
	}
}
#endif
