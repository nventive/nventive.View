#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Xml.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Nventive.View.Extensions
{
	public partial class HtmlTextBlockBehavior
	{
		private static bool _isBold = false;
		private static bool _isItalic = false;
		private static bool _isUnderline = false;
		private static bool _isHyperLink = false;
		private static bool _isUrl = false;
		private static bool _isPhone = false;
		private static bool _isEmail = false;
		private static bool _isHeader = false;
		private static bool _isList = false;
		private static bool _previousHasLinebreak = false;
		private static string _hyperlink = "";
		private static string _phone = "";
		private static string _email = "";
		private static string _bulletCharacter = "\u2022";
		private static Uri _url = default(Uri);

		private static void OnHtmlTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textblock = d as TextBlock;
			textblock.Inlines.Clear();

			var text = e.NewValue as string;

			if (text == null)
			{
				return;
			}

			RenderHtmlText(textblock, text, d);
		}

		private static void OnOpenUriCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textblock = d as TextBlock;
			textblock.Inlines.Clear();

			var text = GetHtmlText(d);

			if (text == null)
			{
				return;
			}

			RenderHtmlText(textblock, text, d);
		}

		private static void RenderHtmlText(TextBlock textblock, string text, DependencyObject sender)
		{
			text = text.Replace("\n", "");
			text = text.Replace("\r", "");
			text = text.Replace("\t", "");
			text = text.Replace("<br>", "\n");
			text = text.Replace("<br/>", "\n");
			text = text.Replace("<br />", "\n");
			text = text.Replace("&nbsp;", " ");

			while (text.Length > 0)
			{
				text = ApplyStyle(textblock, text, sender);
			}
		}

		private static string ApplyStyle(TextBlock textblock, string text, DependencyObject sender)
		{
			var startOfTag = text.IndexOf("<");

			if (startOfTag > 0)
			{
				AddInline(textblock, text.Substring(0, startOfTag), sender);
				text = text.Substring(startOfTag);
			}

			if (startOfTag == -1)
			{
				AddInline(textblock, text, sender);
				return string.Empty;
			}

			return ParseHtmlTag(textblock, text, sender);
		}

		private static string ParseHtmlTag(TextBlock textblock, string text, DependencyObject sender)
		{
			var linebreak = _previousHasLinebreak;
			_previousHasLinebreak = false;

			if (text.StartsWith("<b>", StringComparison.OrdinalIgnoreCase))
			{
				_isBold = true;
				var end = text.IndexOf(">");
				return text.Substring(end + 1);
			}

			if (text.StartsWith("<h3>", StringComparison.OrdinalIgnoreCase))
			{
				_isHeader = true;
				_isBold = true;
				var end = text.IndexOf(">");
				return text.Substring(end + 1);
			}

			if (text.StartsWith("<a", StringComparison.OrdinalIgnoreCase))
			{
				var end = text.IndexOf(">");

				if (!GetDisableLinks(sender))
				{
					var aTag = text.Substring(0, end + 1);
					var aXml = XDocument.Parse(aTag + "</a>");
					var node = aXml?.Element("a");
					var link = node?.Attribute("href")?.Value;

					_url = new Uri(link);
					_isUrl = true;

					if (link.StartsWith("tel:"))
					{
						link = link.Substring(4);
						_phone = link;
						_isPhone = true;
					}
					else if (link.StartsWith("mailto:"))
					{
						link = link.Substring(7);
						_email = link;
						_isEmail = true;
					}
					else
					{
						_hyperlink = link;
						_isHyperLink = true;
					}
				}

				return text.Substring(end + 1);
			}

			if (text.StartsWith("</a>", StringComparison.OrdinalIgnoreCase))
			{
				_isUrl = false;
				_isHyperLink = false;
				_isPhone = false;
				_url = default(Uri);
				_phone = "";
				_hyperlink = "";
				_isEmail = false;
				_email = "";
				return text.Substring(4);
			}

			if (text.StartsWith("<strong>", StringComparison.OrdinalIgnoreCase))
			{
				_isBold = true;
				var end = text.IndexOf(">");
				return text.Substring(end + 1);
			}

			if (text.StartsWith("</b>", StringComparison.OrdinalIgnoreCase))
			{
				_isBold = false;
				return text.Substring(4);
			}

			if (text.StartsWith("</h3>", StringComparison.OrdinalIgnoreCase))
			{
				_isHeader = false;
				_isBold = false;

				var remaining = text.Substring(5);
				if (remaining.Length > 0)
				{
					textblock.Inlines.Add(new LineBreak());
				}

				return remaining;
			}

			if (text.StartsWith("</strong>", StringComparison.OrdinalIgnoreCase))
			{
				_isBold = false;
				return text.Substring(9);
			}

			if (text.StartsWith("<i>", StringComparison.OrdinalIgnoreCase))
			{
				_isItalic = true;
				var end = text.IndexOf(">");
				return text.Substring(end + 1);
			}

			if (text.StartsWith("<em>", StringComparison.OrdinalIgnoreCase))
			{
				_isItalic = true;
				var end = text.IndexOf(">");
				return text.Substring(end + 1);
			}

			if (text.StartsWith("</i>", StringComparison.OrdinalIgnoreCase))
			{
				_isItalic = false;
				return text.Substring(4);
			}

			if (text.StartsWith("</em>", StringComparison.OrdinalIgnoreCase))
			{
				_isItalic = false;
				return text.Substring(5);
			}

			if (text.StartsWith("<u>", StringComparison.OrdinalIgnoreCase))
			{
				_isUnderline = true;
				var end = text.IndexOf(">");
				return text.Substring(end + 1);
			}

			if (text.StartsWith("</u>", StringComparison.OrdinalIgnoreCase))
			{
				_isUnderline = false;
				return text.Substring(4);
			}

			if (text.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
			{
				var end = text.IndexOf(">");
				return text.Substring(end + 1);
			}

			if (text.StartsWith("</p>", StringComparison.OrdinalIgnoreCase))
			{
				var remaining = text.Substring(4);
				if (remaining.Length > 0)
				{
					if (!linebreak)
					{
						textblock.Inlines.Add(new LineBreak());
						_previousHasLinebreak = true;
					}

					textblock.Inlines.Add(new LineBreak());
				}

				return remaining;
			}

			if (text.StartsWith("<ul>", StringComparison.OrdinalIgnoreCase))
			{
				_isList = true;
				var end = text.IndexOf(">");

				return text.Substring(end + 1);
			}

			if (text.StartsWith("<li>", StringComparison.OrdinalIgnoreCase))
			{
				_isList = true;
				var end = text.IndexOf(">");

				return text.Substring(end + 1);
			}

			if (text.StartsWith("</li>", StringComparison.OrdinalIgnoreCase))
			{
				_isList = false;
				var remaining = text.Substring(5);

				if (remaining.Length > 0)
				{
					textblock.Inlines.Add(new LineBreak());
				}

				return remaining;
			}

			if (text.StartsWith("</ul>", StringComparison.OrdinalIgnoreCase))
			{
				_isList = false;

				var remaining = text.Substring(5);
				if (remaining.Length > 0)
				{
					textblock.Inlines.Add(new LineBreak());
					_previousHasLinebreak = true;
				}

				return remaining;
			}

			//remove all unknown html tags
			if (text.StartsWith("<", StringComparison.OrdinalIgnoreCase))
			{
				var end = text.IndexOf(">");
				if (end == -1)
				{
					return string.Empty;
				}

				return text.Substring(end + 1);
			}

			AddInline(textblock, text.Substring(0, 1), sender);
			return text.Substring(1);
		}

		private static void AddInline(TextBlock textblock, string text, DependencyObject sender)
		{
			//decode text to make sure that special characters such as &reg; are displayed properly.
			text = System.Net.WebUtility.HtmlDecode(text);
			var run = default(Inline);

			if (_isList)
			{
				textblock.Inlines.Add(new Run()
				{
					Text = $"{_bulletCharacter} "
				});

				//reset element to make sure there is no other bullet put in the middle of the list item. 
				_isList = false;
			}

			var fontStyle = _isItalic ? FontStyle.Italic : FontStyle.Normal;
			var fontWeight = _isBold ? FontWeights.Bold : (FontWeight)textblock.GetValue(TextBlock.FontWeightProperty);
			var linkFontWeight = _isBold ? FontWeights.Bold : GetHyperlinkDefaultFontWeight(sender);
			var fontFamily = GetFontFamily(textblock, fontWeight, fontStyle);
			var linkFontFamily = GetFontFamily(textblock, linkFontWeight, fontStyle);

			if (_isUnderline)
			{
				run = new Underline();
				((Underline)run).Inlines.Add(new Run()
				{
					Text = text,
					FontStyle = fontStyle,
					FontWeight = linkFontWeight,
					FontFamily = fontFamily
				});
			}
			else if (_isUrl)
			{
				var underlineStyle = GetHyperlinkUnderlineStyle(sender);

				run = new Hyperlink() { UnderlineStyle = underlineStyle };

				var innerRun = new Run()
				{
					Text = text,
					FontStyle = fontStyle,
					FontWeight = linkFontWeight,
					FontFamily = linkFontFamily
				};

				if (GetLinkColor(textblock) != null)
				{
					innerRun.Foreground = GetLinkColor(textblock);
				}

				((Hyperlink)run).Inlines.Add(innerRun);

				if (GetOpenUriCommand(sender) != null)
				{
					var url = _url;
					((Hyperlink)run).Click += (s, e) => OpenUrl(sender, url);
				}
				else
				{
					if (_isHyperLink)
					{
						try
						{
							((Hyperlink)run).NavigateUri = new Uri(_hyperlink);
						}
						catch
						{
							// We don't need to do anything here (Swallow exception)
						}
					}
					else if (_isPhone)
					{
						var phone = _phone;
						((Hyperlink)run).Click += (s, e) => OpenPhone(sender, phone);
					}
					else if (_isEmail)
					{
						var email = _email;
						((Hyperlink)run).Click += (s, e) => OpenEmail(sender, email);
					}
				}
			}
			else
			{
				run = new Run()
				{
					Text = text,
					FontStyle = fontStyle,
					FontWeight = fontWeight,
					FontFamily = fontFamily,
					FontSize = _isHeader ? GetHeaderFontSize(sender) : (double)textblock.GetValue(TextBlock.FontSizeProperty)
				};
			}

			textblock.Inlines.Add(run);
		}

		private static FontFamily GetFontFamily(TextBlock textblock, FontWeight fontWeight, FontStyle fontStyle)
		{
			if (FontWeights.Bold.Equals(fontWeight) && FontStyle.Italic.Equals(fontStyle))
			{
				return GetBoldItalicFontFamily(textblock);
			}
			else if (FontWeights.Bold.Equals(fontWeight))
			{
				return GetBoldFontFamily(textblock);
			}
			else if (FontWeights.SemiBold.Equals(fontWeight))
			{
				return GetSemiBoldFontFamily(textblock);
			}
			else if (FontStyle.Italic.Equals(fontStyle))
			{
				return GetItalicFontFamily(textblock);
			}

			return textblock.GetValue(TextBlock.FontFamilyProperty) as FontFamily ?? GetRegularFontFamily(textblock);
		}

		private static void OpenPhone(DependencyObject sender, string phoneNumber)
		{
			GetOpenPhoneCommand(sender)?.Execute(phoneNumber);
		}

		private static void OpenEmail(DependencyObject sender, string email)
		{
			GetOpenEmailCommand(sender)?.Execute(email);
		}

		private static void OpenUrl(DependencyObject sender, Uri url)
		{
			GetOpenUriCommand(sender)?.Execute(url);
		}
	}
}
#endif
