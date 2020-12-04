#if WINDOWS_UWP || HAS_WINUI || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if HAS_WINUI
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace Chinook.View.Controls
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
