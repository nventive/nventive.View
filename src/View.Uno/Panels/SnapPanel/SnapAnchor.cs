#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.View.Controls
{
	[Flags]
	public enum SnapAnchor
	{
		None = 0x0,
		Near = 0x1,
		Center = 0x2,
		Far = 0x4,
	}
}
#endif
