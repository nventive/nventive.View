#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using Uno.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Uno.Logging;
using System.Reactive.Linq;

namespace Nventive.View.Controls
{
	[TemplatePart(Name = HeaderBackgroundName, Type = typeof(StackPanel))]
	[TemplatePart(Name = HeaderForegroundName, Type = typeof(StackPanel))]
	[TemplatePart(Name = ListViewName, Type = typeof(ListView))]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("", "NV0114", Justification = "Make sense")]

	public partial class ParallaxListView : Control
	{
		public static readonly string TitleHiddenScrollStatusName = "TitleHidden";
		public static readonly string HeaderHiddenScrollStatusName = "HeaderHidden";
		public static readonly string NormalScrollStatusName = "Normal";

		private const int CommandBarHeight = 48;
		private const int TitleOffset = 30;

		private const string HeaderBackgroundName = "PART_HeaderBackground";
		private const string HeaderForegroundName = "PART_HeaderForeground";
		private const string ListViewName = "PART_ListView";

		private FrameworkElement _headerBackground;
		private FrameworkElement _headerForeground;
		private ListView _mainListview;
		private ScrollViewer _mainScrollViewer;

		public ParallaxListView()
		{
			DefaultStyleKey = typeof(ParallaxListView);
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_mainListview.ApplyTemplate();
			_mainScrollViewer = _mainListview.FindFirstChild<ScrollViewer>();

			RegisterEventHandlers();

			UpdateHeaderPosition(this);
			UpdateHeaderOpacity(this);
			UpdateScrollStatus(this);
		}

		private void RegisterEventHandlers()
		{
			Observable.FromEventPattern<EventHandler<ScrollViewerViewChangedEventArgs>, ScrollViewerViewChangedEventArgs>(
				h => _mainScrollViewer.ViewChanged += h,
				h => _mainScrollViewer.ViewChanged -= h,
				new MainDispatcherScheduler(_mainScrollViewer.Dispatcher)
			)
			.SubscribeToElement(
				_mainScrollViewer,
				onNext: input =>
				{
					UpdateHeaderPosition(this);
					UpdateHeaderOpacity(this);
					UpdateScrollStatus(this);
				},
				onError: ex => this.Log().ErrorIfEnabled(() => "Cannot subscribe to _mainScrollViewer.ViewChanged event", ex)
			);
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_headerBackground = this.GetTemplateChild(HeaderBackgroundName) as FrameworkElement;
			_headerForeground = this.GetTemplateChild(HeaderForegroundName) as FrameworkElement;

			_mainListview = this.GetTemplateChild(ListViewName) as ListView;

			if (_headerBackground == null)
			{
				throw new System.ArgumentException($"Missing template part with name: {HeaderBackgroundName}");
			}

			if (_headerForeground == null)
			{
				throw new System.Exception($"Missing template part with name: {HeaderForegroundName}");
			}

			if (_mainListview == null)
			{
				throw new System.Exception($"Missing template part with name: {ListViewName}");
			}

			_headerBackground.RenderTransform = new TranslateTransform();
			_headerForeground.RenderTransform = new TranslateTransform();
		}

		private void UpdateHeaderPosition(object sender)
		{
			var headerBackgroundOffset = _mainScrollViewer.VerticalOffset - (_mainScrollViewer.VerticalOffset / 1.5);
			var headerForegroundOffset = _mainScrollViewer.VerticalOffset - (_mainScrollViewer.VerticalOffset / 2);

			if (_mainScrollViewer.VerticalOffset <= HeaderHeight)
			{
				_headerBackground.Visibility = Visibility.Visible;
				_headerForeground.Visibility = Visibility.Visible;

				if (headerBackgroundOffset < _mainScrollViewer.VerticalOffset)
				{
					(_headerBackground.RenderTransform as TranslateTransform).Y = -headerBackgroundOffset;
					(_headerForeground.RenderTransform as TranslateTransform).Y = -headerForegroundOffset;
				}
				else
				{
					(_headerBackground.RenderTransform as TranslateTransform).Y = 0;
					(_headerForeground.RenderTransform as TranslateTransform).Y = 0;
				}
			}
			else
			{
				_headerBackground.Visibility = Visibility.Collapsed;
				_headerForeground.Visibility = Visibility.Collapsed;
			}
		}

		private void UpdateHeaderOpacity(object sender)
		{
			if (!IsNonInteractiveHeaderForegroundFading)
			{
				return;
			}

			var opacity = 1 - (_mainScrollViewer.VerticalOffset / HeaderHeight);
			_headerForeground.Opacity = opacity;
		}

		private void UpdateScrollStatus(object sender)
		{
			if (_mainScrollViewer.VerticalOffset - TitleOffset > HeaderHeight)
			{
				ScrollStatus = TitleHiddenScrollStatusName;
			}
			else if (_mainScrollViewer.VerticalOffset > HeaderHeight - CommandBarHeight)
			{
				ScrollStatus = HeaderHiddenScrollStatusName;
			}
			else
			{
				ScrollStatus = NormalScrollStatusName;
			}
		}
	}
}
#endif
