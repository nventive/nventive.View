#if __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Core;
#if WINDOWS_UWP
using Windows.Foundation.Metadata;
#endif

namespace Chinook.View.Controls
{
    public partial class MembershipCardControl
    {
		private void SetBrightness(double brightnessLevel)
		{
			RunIfSupported(brightnessOverride =>
			{
				brightnessOverride.SetBrightnessLevel(brightnessLevel, DisplayBrightnessOverrideOptions.None);

				brightnessOverride.StartOverride();
			});
		}

		private void ResetBrightness()
		{
			RunIfSupported(brightnessOverride => brightnessOverride.StopOverride());
		}

		private void RunIfSupported(Action<BrightnessOverride> operation)
		{
			if (IsBrightnessOverrideSupported())
			{
				_ = Dispatcher.RunTaskAsync(CoreDispatcherPriority.Normal, async () =>
				{
					var brightnessOverride = BrightnessOverride.GetForCurrentView();
					operation(brightnessOverride);
				});
			}
		}

		private bool IsBrightnessOverrideSupported()
		{
			var isSupported = true;

#if WINDOWS_UWP
			isSupported = ApiInformation.IsApiContractPresent("Windows.Graphics.Display.BrightnessOverride", 1);
			if (isSupported)
			{
				var brightnessOverride = BrightnessOverride.GetForCurrentView();
				isSupported = brightnessOverride.IsSupported;
			}
#endif
			return isSupported;
		}
	}
}
#endif
