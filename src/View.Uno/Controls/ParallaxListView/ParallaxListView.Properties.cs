#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nventive.View.Controls
{
	public partial class ParallaxListView : Control
	{
		#region HeaderBackgroundTemplate Property

		public DataTemplate HeaderBackgroundTemplate
		{
			get { return (DataTemplate)GetValue(HeaderBackgroundTemplateProperty); }
			set { SetValue(HeaderBackgroundTemplateProperty, value); }
		}

		public static readonly DependencyProperty HeaderBackgroundTemplateProperty =
			DependencyProperty.Register("HeaderBackgroundTemplate", typeof(DataTemplate), typeof(ParallaxListView), new PropertyMetadata(null));

		#endregion

		#region HeaderHeight Property

		public double HeaderHeight
		{
			get { return (double)GetValue(HeaderHeightProperty); }
			set { SetValue(HeaderHeightProperty, value); }
		}

		public static readonly DependencyProperty HeaderHeightProperty =
			DependencyProperty.Register("HeaderHeight", typeof(double), typeof(ParallaxListView), new PropertyMetadata(1));

		#endregion

		#region IsHeaderForegroundFading Property

		public bool IsNonInteractiveHeaderForegroundFading
		{
			get { return (bool)GetValue(IsNonInteractiveHeaderForegroundFadingProperty); }
			set { SetValue(IsNonInteractiveHeaderForegroundFadingProperty, value); }
		}

		public static readonly DependencyProperty IsNonInteractiveHeaderForegroundFadingProperty =
			DependencyProperty.Register("IsNonInteractiveHeaderForegroundFading", typeof(bool), typeof(ParallaxListView), new PropertyMetadata(false));

		#endregion

		#region ItemCommand Property

		public ICommand ItemCommand
		{
			get { return (ICommand)GetValue(ItemCommandProperty); }
			set { SetValue(ItemCommandProperty, value); }
		}

		public static readonly DependencyProperty ItemCommandProperty =
			DependencyProperty.Register("ItemCommand", typeof(ICommand), typeof(ParallaxListView), new PropertyMetadata(null));

		#endregion

		#region ItemContainerStyle Property

		public Style ItemContainerStyle
		{
			get { return (Style)GetValue(ItemContainerStyleProperty); }
			set { SetValue(ItemContainerStyleProperty, value); }
		}

		public static readonly DependencyProperty ItemContainerStyleProperty =
			DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ParallaxListView), new PropertyMetadata(null));

		#endregion

		#region ItemsSource Property

		public object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(object), typeof(ParallaxListView), new PropertyMetadata(null));

		#endregion

		#region ItemTemplate Property

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ParallaxListView), new PropertyMetadata(null));

		#endregion

		#region ItemTemplateSelector Property

		public DataTemplateSelector ItemTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
			set { SetValue(ItemTemplateSelectorProperty, value); }
		}

		public static readonly DependencyProperty ItemTemplateSelectorProperty =
			DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(ParallaxListView), new PropertyMetadata(null));

		#endregion

		#region NonInteractiveHeaderForegroundTemplate Property

		public DataTemplate NonInteractiveHeaderForegroundTemplate
		{
			get { return (DataTemplate)GetValue(NonInteractiveHeaderForegroundTemplateProperty); }
			set { SetValue(NonInteractiveHeaderForegroundTemplateProperty, value); }
		}

		public static readonly DependencyProperty NonInteractiveHeaderForegroundTemplateProperty =
			DependencyProperty.Register("NonInteractiveHeaderForegroundTemplate", typeof(DataTemplate), typeof(ParallaxListView), new PropertyMetadata(null));

		#endregion

		#region ScrollStatus Property

		public string ScrollStatus
		{
			get { return (string)GetValue(ScrollStatusProperty); }
			set { SetValue(ScrollStatusProperty, value); }
		}

		public static readonly DependencyProperty ScrollStatusProperty =
			DependencyProperty.Register("ScrollStatus", typeof(string), typeof(ParallaxListView), new PropertyMetadata("Collasped"));

		#endregion
	}
}
#endif
