#if WINDOWS_UWP || HAS_WINUI
using System;
using System.Collections.Generic;
using System.Text;
#if HAS_WINUI
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml.Controls;
#endif

namespace Chinook.View.Controls
{
    public partial class LandscapeUprightPanel : Grid
    {
    }
}
#endif
