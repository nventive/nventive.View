#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Extensions
{
	/// <summary>
	/// BindableVisualState is a class that exposes attachable properties named VisualStateName that lets you bind a string on a Control to set a VisualState.
	/// This allows you to control the visual states of your view from a ViewModel or anything that can be bound from.
	/// You can use this on Page and UserControl to add animations between states of your content.
	/// </summary>
	public class BindableVisualState
	{
		public static string GetVisualStateName(DependencyObject obj)
		{
			return (string)obj.GetValue(VisualStateNameProperty);
		}

		public static void SetVisualStateName(DependencyObject obj, string value)
		{
			obj.SetValue(VisualStateNameProperty, value);
		}

		public static readonly DependencyProperty VisualStateNameProperty =
			DependencyProperty.RegisterAttached("VisualStateName", typeof(string), typeof(BindableVisualState), new PropertyMetadata(null, OnVisualStateNameChanged));

		private static void OnVisualStateNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = (Control)d;
			var visualStateName = (string)e.NewValue;

			if (control != null && visualStateName != null)
			{
				VisualStateManager.GoToState(control, visualStateName, true);
			}
		}

		// This second property is used to not have to merge all potential binding source.
		// It's particularly useful when having multiple VisualStateGroups.
		public static string GetVisualStateName2(DependencyObject obj)
		{
			return (string)obj.GetValue(VisualStateName2Property);
		}

		public static void SetVisualStateName2(DependencyObject obj, string value)
		{
			obj.SetValue(VisualStateName2Property, value);
		}

		public static readonly DependencyProperty VisualStateName2Property =
			DependencyProperty.RegisterAttached("VisualStateName2", typeof(string), typeof(BindableVisualState), new PropertyMetadata(null, OnVisualStateNameChanged));

		// This third property is also used to not have to merge all potential binding source.
		// It's particularly useful when having multiple VisualStateGroups.
		public static string GetVisualStateName3(DependencyObject obj)
		{
			return (string)obj.GetValue(VisualStateName3Property);
		}

		public static void SetVisualStateName3(DependencyObject obj, string value)
		{
			obj.SetValue(VisualStateName3Property, value);
		}

		public static readonly DependencyProperty VisualStateName3Property =
			DependencyProperty.RegisterAttached("VisualStateName3", typeof(string), typeof(BindableVisualState), new PropertyMetadata(null, OnVisualStateNameChanged));

		// This fourth property is, you guessed it, used to not have to merge all potential binding source.
		// It's particularly useful when having multiple VisualStateGroups.
		public static string GetVisualStateName4(DependencyObject obj)
		{
			return (string)obj.GetValue(VisualStateName4Property);
		}

		public static void SetVisualStateName4(DependencyObject obj, string value)
		{
			obj.SetValue(VisualStateName4Property, value);
		}

		public static readonly DependencyProperty VisualStateName4Property =
			DependencyProperty.RegisterAttached("VisualStateName4", typeof(string), typeof(BindableVisualState), new PropertyMetadata(null, OnVisualStateNameChanged));
	}
}
#endif
