#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Nventive.View.Controls
{
	public class SnapPoint
	{
		public bool IsSnapping { get; set; }
		public GridLength Position { get; set; }
	}
}
#endif
