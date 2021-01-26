#if __ANDROID__ || __IOS__ || __WASM__ || WINDOWS_UWP
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.Logging;
using System.ComponentModel;

namespace Nventive.View.Controls
{
	/// <summary>
	/// A custom panel which maintains proportion on the child controls.
	/// </summary>
	/// <remarks>
	/// Common Ratios:
	///		21:9 = 2.333
	///		18.5:9 = 2.055
	///		16:9 = 1.777
	///		8:5 = 1.6
	///		4:3 = 1.333
	///		3:2 = 1.5
	/// </remarks>
	public partial class ProportionalPanel : Panel
	{
		public enum ProportionMode
		{
			HeightFollowsWidth,
			WidthFollowsHeight
		}

		[DefaultValue(ProportionMode.HeightFollowsWidth)]
		public ProportionMode Mode { get; set; }

		[DefaultValue(1.0)]
		public double Ratio { get; set; } = 1.0;

		protected override Size MeasureOverride(Size availableSize)
		{
			this.Log().Debug(() => $"Measuring UI element (available height: '{availableSize.Height}', available width: '{availableSize.Width}').");

			var calculatedWidth = availableSize.Height * Ratio;
			var calculatedHeight = availableSize.Width / Ratio;

			// Assign Height/Width according to ProportionMode
			if (Mode == ProportionMode.HeightFollowsWidth)
			{
				if (calculatedHeight <= availableSize.Height)
				{
					availableSize.Height = calculatedHeight;
				}
				else
				{
					availableSize.Width = calculatedWidth;
				}
			}
			else if (Mode == ProportionMode.WidthFollowsHeight)
			{
				if (calculatedWidth <= availableSize.Width)
				{
					availableSize.Width = calculatedWidth;
				}
				else
				{
					availableSize.Height = calculatedHeight;
				}
			}

#if __IOS__ || __ANDROID__
			base.MeasureOverride(availableSize);
#else
			foreach (UIElement child in Children)
			{
				child.Measure(availableSize);
			}
#endif
			this.Log().Info(() => "Return available size.");

			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
#if __IOS__ || __ANDROID__
			return base.ArrangeOverride(finalSize);
#else
			foreach (UIElement child in Children)
			{
				child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
			}

			this.Log().Info(() => "Arrange final size.");

			return finalSize;
#endif
		}
	}
}
#endif
