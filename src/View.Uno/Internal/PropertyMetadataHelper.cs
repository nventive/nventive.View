#if WINDOWS_UWP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Nventive.View
{
	internal class PropertyMetadataHelper
	{
		public PropertyMetadataHelper(PropertyChangedCallback propertyChangedCallback = null, object defaultValue = null)
		{
			DefaultValue = defaultValue;
			Callback = propertyChangedCallback;
		}

		public static implicit operator PropertyMetadata(PropertyMetadataHelper value)
		{
			if (value.Callback == null)
			{
				return new PropertyMetadata(value.DefaultValue);
			}
			else
			{
				return new PropertyMetadata(value.DefaultValue, value.Callback);
			}
		}

		public object DefaultValue { get; set; }

		public PropertyChangedCallback Callback { get; set; }
	}
}
#endif
