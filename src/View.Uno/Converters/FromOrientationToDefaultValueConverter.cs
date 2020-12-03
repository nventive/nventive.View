#if WINDOWS_UWP
// TODO: Remove above if __ANDROID__ || __IOS__ implemented.
using System;
using System.Collections.Generic;
using System.Text;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#elif __ANDROID__ || __IOS__
// TODO
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace Chinook.View.Converters
{
	/// <summary>
	/// This converter allows mapping an <seealso cref="Orientation"/> to any custom value.
	/// </summary>
	/// <remarks><para>Because an Orientation rarely (and shouldn't) comes from a view-model, this
	/// converter doesn't support null values. If null comes in, null comes out.</para>
	/// <para>This converter can be useful to customize a 
	/// <see cref="nVentive.Chinook.Views.Controls.RichTextCoverPage">RichTextCoverPage</see> or 
	/// <see cref="nVentive.Chinook.Views.Controls.RichTextOverflowPage">RichTextOverflowPage</see>, 
	/// binding to the page's <see cref="nVentive.Chinook.Views.Controls.RichTextPage.Orientation">Orientation</see>.</para></remarks>
	public class FromOrientationToDefaultValueConverter : ConverterBase
	{
		public object HorizontalValue { get; set; }
		public object VerticalValue { get; set; }

		protected override object Convert(object value, Type targetType, object parameter)
		{
			if(value is Orientation)
			{
				return ((Orientation)value) == Orientation.Horizontal
					? this.HorizontalValue
					: this.VerticalValue;
			}

			return null;
		}
	}
}
#endif
