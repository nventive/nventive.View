#if !__ANDROID__ && !__IOS__ && !__WASM__
using System;
using System.Linq;
using Uno.Extensions;
using System.Text.RegularExpressions;
using System.Collections.Generic;
#if !WINDOWS_UWP
using System.Windows;
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Nventive.View.Extensions
{
	public class TextBlockBehavior
	{
		private static Func<int, Regex> _getRegexForLength = GetRegexForLength;

		static TextBlockBehavior()
		{
			_getRegexForLength = _getRegexForLength.AsLockedMemoized();
		}

		public static readonly DependencyProperty WrapWithOverflowTextProperty =
			DependencyProperty.RegisterAttached("WrapWithOverflowText", typeof(string), typeof(TextBlockBehavior), new PropertyMetadata(string.Empty, (d, e) => WrapText(d, e.NewValue as string)));

		public static string GetWrapWithOverflowText(TextBlock textBlock)
		{
			return textBlock.GetValue(WrapWithOverflowTextProperty) as string;
		}

		public static void SetWrapWithOverflowText(TextBlock textBlock, string text)
		{
			textBlock.SetValue(WrapWithOverflowTextProperty, text);
		}

		public static void WrapText(object obj, string text)
		{
			var textBlock = obj as TextBlock;

			if (textBlock != null)
			{
				textBlock.TextWrapping = TextWrapping.NoWrap;
				textBlock.Text = WrapWithOverflow(text, 10);
			}
		}


		public static string WrapWithOverflow(string text, int length)
		{
			var parts = _getRegexForLength(length).Matches(text)
								.Cast<Match>()
								.SelectMany(match => SelectMany(match.Groups, "part", "part2", "part1")) // Part2 before 1 for .Reverse()
								.Trim()
								.Select(group => group.Value.Trim())
								.Where(item => item.HasValue())
								.Reverse();

			return ToLines(parts);
		}

		private static Regex GetRegexForLength(int length)
		{
			var pattern = "\\s+(?<part>.{1," + length + "})|(?<part1>\\S+-)(?<part2>\\S+)|(?<part>\\S+)";

			return new Regex(pattern, RegexOptions.RightToLeft);
		}

		public static string ToLines(IEnumerable<string> parts)
		{
			return String.Join(Environment.NewLine, parts.ToArray());
		}

		public static IEnumerable<Group> SelectMany(GroupCollection groups, params string[] keys)
		{
			return keys.Select(key => groups[key]);
		}

	}
}
#endif
