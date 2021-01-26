#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Extensions
{
	/// <summary>
	/// Provides attached properties on UserControl or Page to handle StatusBar
	/// </summary>
	public class PageBehavior
	{
		#region StatusBarVisibility
		public static Visibility GetStatusBarVisibility(UserControl obj)
		{
			return (Visibility)obj.GetValue(StatusBarVisibilityProperty);
		}

		public static async void SetStatusBarVisibility(UserControl obj, Visibility value)
		{
			// We change the property here instead of the property changed callback because the status bar is shared for all pages, but the attached property isn't.
			// i.e. You don't get to the property changed callback if you set Visible for a page because the default value is Visible.
			var statusBar = GetStatusBar();
			if (statusBar != null)
			{
				if (value == Visibility.Visible)
				{
					await statusBar.ShowAsync();
				}
				else
				{
					await statusBar.HideAsync();
				}
			}

			obj.SetValue(StatusBarVisibilityProperty, value);
		}

		public static readonly DependencyProperty StatusBarVisibilityProperty =
			DependencyProperty.RegisterAttached("StatusBarVisibility", typeof(Visibility), typeof(PageBehavior), new PropertyMetadata(Visibility.Visible));
		#endregion

		#region StatusBarForegroundColor
		public static Color GetStatusBarForegroundColor(UserControl obj)
		{
			return (Color)obj.GetValue(StatusBarForegroundColorProperty);
		}

		public static void SetStatusBarForegroundColor(UserControl obj, Color value)
		{
			// We change the property here instead of the property changed callback because the status bar is shared for all pages, but the attached property isn't.
			// i.e. Same as above but with null instead of Visible.
			var statusBar = GetStatusBar();
			if (statusBar != null)
			{
				statusBar.ForegroundColor = value;
			}

			obj.SetValue(StatusBarForegroundColorProperty, value);
		}

		public static readonly DependencyProperty StatusBarForegroundColorProperty =
			DependencyProperty.RegisterAttached("StatusBarForegroundColor", typeof(Color), typeof(PageBehavior), new PropertyMetadata(null));
		#endregion

		private static bool IsStatusBarSupported => Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");

		private static StatusBar GetStatusBar()
		{
			return IsStatusBarSupported ? StatusBar.GetForCurrentView() : null;
		}
	}
}
#endif
