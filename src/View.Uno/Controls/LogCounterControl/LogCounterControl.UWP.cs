#if WINDOWS_UWP
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Uno.Disposables;

namespace Chinook.View.Controls
{
	public sealed partial class LogCounterControl : ContentControl, ILogger
	{
		private readonly Popup _popup = new Popup();
		private bool _isViewReady;
		private FrameworkElement _warningIndicator;
		private TextBlock _warningCounter;
		private FrameworkElement _errorIndicator;
		private TextBlock _errorCounter;
		private ulong _warningCount = 0;
		private ulong _errorCount = 0;

		public LogCounterControl()
		{
			DefaultStyleKey = typeof(LogCounterControl);
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_warningIndicator = (FrameworkElement)this.GetTemplateChild("PART_WarningIndicator");
			_warningCounter = (TextBlock)this.GetTemplateChild("PART_WarningCounter");
			_errorIndicator = (FrameworkElement)this.GetTemplateChild("PART_ErrorIndicator");
			_errorCounter = (TextBlock)this.GetTemplateChild("PART_ErrorCounter");

			_isViewReady = true;
		}

		public void Open()
		{
			_popup.Child = this;
			_popup.IsOpen = true;

			SizeChanged += OnSizeChanged;
			Window.Current.SizeChanged += OnWindowSizeChanged;
		}

		private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
		{
			UpdatePosition();
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdatePosition();
		}

		private void UpdatePosition()
		{
			if (Window.Current.Content is FrameworkElement content)
			{
				_popup.HorizontalOffset = content.ActualWidth - ActualWidth;
				_popup.VerticalOffset = content.ActualHeight - ActualHeight;
			}
		}

		bool ILogger.IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Warning;

		public IDisposable BeginScope<TState>(TState state) => Disposable.Empty;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!_isViewReady || logLevel < LogLevel.Warning)
			{
				return;
			}

			_ = Dispatcher.RunTaskAsync(CoreDispatcherPriority.Normal, async () =>
			{
				if (logLevel == LogLevel.Warning)
				{
					_warningCounter.Text = (++_warningCount).ToString(CultureInfo.InvariantCulture);
					_warningIndicator.Visibility = Visibility.Visible;
				}
				else
				{
					_errorCounter.Text = (++_errorCount).ToString(CultureInfo.InvariantCulture);
					_errorIndicator.Visibility = Visibility.Visible;
				}
			});
		}
	}

	public class LogCounterControlProvider : ILoggerProvider
	{
		private LogCounterControl _logger;

		public ILogger CreateLogger(string categoryName)
		{
			if (_logger == null)
			{
				_logger = new LogCounterControl();

				_logger.Open();
			}

			return _logger;
		}

		public void Dispose()
		{

		}
	}
}
#endif
