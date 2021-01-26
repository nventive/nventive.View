#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Nventive.View.Controls
{
	public static partial class ResetViewPortZoomControlBehavior
	{
		#region ResetTrigger ATTACHED PROPERTY

		public static object GetResetTrigger(ZoomControl zoomControl)
		{
			return (object)zoomControl.GetValue(ResetTriggerProperty);
		}

		public static void SetResetTrigger(ZoomControl zoomControl, object value)
		{
			zoomControl.SetValue(ResetTriggerProperty, value);
		}

		public static readonly DependencyProperty ResetTriggerProperty =
			DependencyProperty.RegisterAttached("ResetTrigger", typeof(object), typeof(ResetViewPortZoomControlBehavior), new PropertyMetadata(null, OnResetTriggerChanged));

		private static void OnResetTriggerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			(sender as ZoomControl).ResetViewPort();
		}

		#endregion
	}
}
#endif