#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Controls
{
	public partial class ImageSlideshow : Control
	{
		#region ItemsSource DependencyProperty
		public object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(object), typeof(ImageSlideshow), new PropertyMetadata(new object[0], (s, e) => ((ImageSlideshow)s)?.OnItemsSourceChanged()));
		#endregion

		#region SelectedIndex DependencyProperty
		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ImageSlideshow), new PropertyMetadata(0, (s, e) => ((ImageSlideshow)s)?.OnSelectedIndexChanged()));
		#endregion

		#region ItemTemplate DependencyProperty
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ImageSlideshow), new PropertyMetadata(default(DataTemplate), (s, e) => ((ImageSlideshow)s)?.Update()));
		#endregion

		#region ItemTemplateSelector Dependency Property
		public DataTemplateSelector ItemTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
			set { SetValue(ItemTemplateSelectorProperty, value); }
		}

		public static readonly DependencyProperty ItemTemplateSelectorProperty =
			DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(ImageSlideshow), new PropertyMetadata(default(DataTemplateSelector)));
		#endregion

		#region IndexIndicatorTemplate DependencyProperty
		public DataTemplate IndexIndicatorTemplate
		{
			get { return (DataTemplate)GetValue(IndexIndicatorTemplateProperty); }
			set { SetValue(IndexIndicatorTemplateProperty, value); }
		}

		public static readonly DependencyProperty IndexIndicatorTemplateProperty =
			DependencyProperty.Register("IndexIndicatorTemplate", typeof(DataTemplate), typeof(ImageSlideshow), new PropertyMetadata(default(DataTemplate), (s, e) => ((ImageSlideshow)s)?.Update()));
		#endregion

		#region SelectedIndexIndicatorTemplate DependencyProperty
		public DataTemplate SelectedIndexIndicatorTemplate
		{
			get { return (DataTemplate)GetValue(SelectedIndexIndicatorTemplateProperty); }
			set { SetValue(SelectedIndexIndicatorTemplateProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexIndicatorTemplateProperty =
			DependencyProperty.Register("SelectedIndexIndicatorTemplate", typeof(DataTemplate), typeof(ImageSlideshow), new PropertyMetadata(default(DataTemplate), (s, e) => ((ImageSlideshow)s)?.Update()));
		#endregion

		#region ShouldPreserveSelectedIndex DependencyProperty
		public bool ShouldPreserveSelectedIndex
		{
			get { return (bool)GetValue(ShouldPreserveSelectedIndexProperty); }
			set { SetValue(ShouldPreserveSelectedIndexProperty, value); }
		}

		public static readonly DependencyProperty ShouldPreserveSelectedIndexProperty =
			DependencyProperty.Register("ShouldPreserveSelectedIndex", typeof(bool), typeof(ImageSlideshow), new PropertyMetadata(false, (s, e) => ((ImageSlideshow)s)?.Update()));
		#endregion

		#region AutoRotate DependencyProperty
		public bool AutoRotate
		{
			get { return (bool)GetValue(AutoRotateProperty); }
			set { SetValue(AutoRotateProperty, value); }
		}

		public static readonly DependencyProperty AutoRotateProperty =
			DependencyProperty.Register("AutoRotate", typeof(bool), typeof(ImageSlideshow), new PropertyMetadata(false, (s, e) => ((ImageSlideshow)s)?.OnAutoRotateChanged()));
		#endregion

		#region DisplayTime for each Items
		public int DisplayTime
		{
			get { return (int)this.GetValue(DisplayTimeProperty); }
			set { this.SetValue(DisplayTimeProperty, value); }
		}

		public static readonly DependencyProperty DisplayTimeProperty =
			DependencyProperty.Register("DisplayTime", typeof(int), typeof(ImageSlideshow), new PropertyMetadata(2000));
		#endregion

		#region RewindTime for each Items
		public int RewindTime
		{
			get { return (int)this.GetValue(RewindTimeProperty); }
			set { this.SetValue(RewindTimeProperty, value); }
		}

		public static readonly DependencyProperty RewindTimeProperty =
			DependencyProperty.Register("RewindTime", typeof(int), typeof(ImageSlideshow), new PropertyMetadata(80));
		#endregion

		#region AutoRotateRewindEnabled DependencyProperty
		public bool AutoRotateRewindEnabled
		{
			get { return (bool)GetValue(AutoRotateRewindEnabledProperty); }
			set { SetValue(AutoRotateRewindEnabledProperty, value); }
		}

		public static readonly DependencyProperty AutoRotateRewindEnabledProperty =
			DependencyProperty.Register("AutoRotateRewindEnabled", typeof(bool), typeof(ImageSlideshow), new PropertyMetadata(true));
		#endregion
	}
}
#endif
