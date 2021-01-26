#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Nventive.View.Controls
{
	public partial class PeekingFlipViewItem : SelectorItem
	{
		public PeekingFlipViewItem()
		{
			DefaultStyleKey = typeof(PeekingFlipViewItem);
		}
	}
}
#endif
