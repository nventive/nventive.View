#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System.Windows.Input;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Nventive.View.Extensions
{
	public partial class HtmlTextBlockBehavior
	{
		public static bool GetDisableLinks(DependencyObject obj)
		{
			return (bool)obj.GetValue(DisableLinksProperty);
		}

		public static void SetDisableLinks(DependencyObject obj, bool value)
		{
			obj.SetValue(DisableLinksProperty, value);
		}

		public static readonly DependencyProperty DisableLinksProperty =
			DependencyProperty.RegisterAttached(
				"DisableLinks",
				typeof(bool),
				typeof(HtmlTextBlockBehavior),
				new PropertyMetadata(false));

		public static Brush GetLinkColor(DependencyObject obj)
		{
			return (Brush)obj.GetValue(LinkColorProperty);
		}

		public static void SetLinkColor(DependencyObject obj, Brush value)
		{
			obj.SetValue(LinkColorProperty, value);
		}

		public static readonly DependencyProperty LinkColorProperty =
			DependencyProperty.RegisterAttached(
				"LinkColor",
				typeof(Brush),
				typeof(HtmlTextBlockBehavior),
				new PropertyMetadata(default(Brush)));

		public static ICommand GetOpenPhoneCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(OpenPhoneCommandProperty);
		}

		public static void SetOpenPhoneCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(OpenPhoneCommandProperty, value);
		}

		public static readonly DependencyProperty OpenPhoneCommandProperty =
			DependencyProperty.RegisterAttached(
				"OpenPhoneCommand",
				typeof(ICommand),
				typeof(HtmlTextBlockBehavior),
				new PropertyMetadata(null));

		public static ICommand GetOpenEmailCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(OpenEmailCommandProperty);
		}

		public static void SetOpenEmailCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(OpenEmailCommandProperty, value);
		}

		public static readonly DependencyProperty OpenEmailCommandProperty =
			DependencyProperty.RegisterAttached(
				"OpenEmailCommand",
				typeof(ICommand),
				typeof(HtmlTextBlockBehavior),
				new PropertyMetadata(null));

		public static ICommand GetOpenUriCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(OpenUriCommandProperty);
		}

		public static void SetOpenUriCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(OpenUriCommandProperty, value);
		}

		public static readonly DependencyProperty OpenUriCommandProperty =
			DependencyProperty.RegisterAttached("OpenUriCommand", typeof(ICommand), typeof(HtmlTextBlockBehavior), new PropertyMetadata(null, OnOpenUriCommandChanged));

		public static FontFamily GetRegularFontFamily(DependencyObject obj)
		{
			return (FontFamily)obj.GetValue(RegularFontFamilyProperty);
		}

		public static void SetRegularFontFamily(DependencyObject obj, FontFamily value)
		{
			obj.SetValue(RegularFontFamilyProperty, value);
		}

		public static readonly DependencyProperty RegularFontFamilyProperty =
			DependencyProperty.RegisterAttached("RegularFontFamily", typeof(FontFamily), typeof(HtmlTextBlockBehavior), new PropertyMetadata(default(FontFamily)));

		public static int GetHeaderFontSize(DependencyObject obj)
		{
			return (int)obj.GetValue(HeaderFontSizeProperty);
		}

		public static void SetHeaderFontSize(DependencyObject obj, int value)
		{
			obj.SetValue(HeaderFontSizeProperty, value);
		}

		public static readonly DependencyProperty HeaderFontSizeProperty =
			DependencyProperty.RegisterAttached("HeaderFontSize", typeof(int), typeof(HtmlTextBlockBehavior), new PropertyMetadata(default(int)));

		public static FontFamily GetBoldFontFamily(DependencyObject obj)
		{
			return (FontFamily)obj.GetValue(BoldFontFamilyProperty);
		}

		public static void SetBoldFontFamily(DependencyObject obj, FontFamily value)
		{
			obj.SetValue(BoldFontFamilyProperty, value);
		}

		public static readonly DependencyProperty BoldFontFamilyProperty =
			DependencyProperty.RegisterAttached("BoldFontFamily", typeof(FontFamily), typeof(HtmlTextBlockBehavior), new PropertyMetadata(default(FontFamily)));

		public static FontFamily GetSemiBoldFontFamily(DependencyObject obj)
		{
			return (FontFamily)obj.GetValue(SemiBoldFontFamilyProperty);
		}

		public static void SetSemiBoldFontFamily(DependencyObject obj, FontFamily value)
		{
			obj.SetValue(SemiBoldFontFamilyProperty, value);
		}

		public static readonly DependencyProperty SemiBoldFontFamilyProperty =
			DependencyProperty.RegisterAttached("SemiBoldFontFamily", typeof(FontFamily), typeof(HtmlTextBlockBehavior), new PropertyMetadata(default(FontFamily)));

		public static FontFamily GetItalicFontFamily(DependencyObject obj)
		{
			return (FontFamily)obj.GetValue(ItalicFontFamilyProperty);
		}

		public static void SetItalicFontFamily(DependencyObject obj, FontFamily value)
		{
			obj.SetValue(ItalicFontFamilyProperty, value);
		}

		public static readonly DependencyProperty ItalicFontFamilyProperty =
			DependencyProperty.RegisterAttached("ItalicFontFamily", typeof(FontFamily), typeof(HtmlTextBlockBehavior), new PropertyMetadata(default(FontFamily)));

		public static FontFamily GetBoldItalicFontFamily(DependencyObject obj)
		{
			return (FontFamily)obj.GetValue(BoldItalicFontFamilyProperty);
		}

		public static void SetBoldItalicFontFamily(DependencyObject obj, FontFamily value)
		{
			obj.SetValue(BoldItalicFontFamilyProperty, value);
		}

		public static readonly DependencyProperty BoldItalicFontFamilyProperty =
			DependencyProperty.RegisterAttached("BoldItalicFontFamily", typeof(FontFamily), typeof(HtmlTextBlockBehavior), new PropertyMetadata(default(FontFamily)));

		public static UnderlineStyle GetHyperlinkUnderlineStyle(DependencyObject obj)
		{
			return (UnderlineStyle)obj.GetValue(HyperlinkUnderlineStyleProperty);
		}

		public static void SetHyperlinkUnderlineStyle(DependencyObject obj, UnderlineStyle value)
		{
			obj.SetValue(HyperlinkUnderlineStyleProperty, value);
		}

		public static readonly DependencyProperty HyperlinkUnderlineStyleProperty =
			DependencyProperty.RegisterAttached("HyperlinkUnderlineStyle", typeof(UnderlineStyle), typeof(HtmlTextBlockBehavior), new PropertyMetadata(UnderlineStyle.None));

		public static FontWeight GetHyperlinkDefaultFontWeight(DependencyObject obj)
		{
			return (FontWeight)obj.GetValue(HyperlinkDefaultFontWeightProperty);
		}

		public static void SetHyperlinkDefaultFontWeight(DependencyObject obj, FontWeight value)
		{
			obj.SetValue(HyperlinkDefaultFontWeightProperty, value);
		}

		public static readonly DependencyProperty HyperlinkDefaultFontWeightProperty =
			DependencyProperty.RegisterAttached("HyperlinkDefaultFontWeight", typeof(FontWeight), typeof(HtmlTextBlockBehavior), new PropertyMetadata(FontWeights.Normal));

		public static string GetHtmlText(DependencyObject obj)
		{
			return (string)obj.GetValue(HtmlTextProperty);
		}

		public static void SetHtmlText(DependencyObject obj, string value)
		{
			obj.SetValue(HtmlTextProperty, value);
		}

		public static readonly DependencyProperty HtmlTextProperty =
			DependencyProperty.RegisterAttached(
				"HtmlText",
				typeof(string),
				typeof(HtmlTextBlockBehavior),
				new PropertyMetadata(null, OnHtmlTextChanged));
	}
}
#endif
