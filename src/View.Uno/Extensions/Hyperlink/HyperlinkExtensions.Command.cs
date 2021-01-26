#if WINDOWS_UWP || __ANDROID__ || __IOS__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace Nventive.View.Extensions
{
	public partial class HyperlinkExtensions
	{
		public static ICommand GetCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(CommandProperty);
		}

		public static void SetCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(CommandProperty, value);
		}

		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(HyperlinkExtensions), new PropertyMetadata(null, OnCommandChanged));

		public static object GetCommandParameter(DependencyObject obj)
		{
			return obj.GetValue(CommandParameterProperty);
		}

		public static void SetCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(CommandParameterProperty, value);
		}

		/// <summary>
		/// Value which can be used to specify a parameter to use alongside Command when executing.
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(HyperlinkExtensions), new PropertyMetadata(null));

		private static void OnCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var hyperlink = (Hyperlink)o;
			
			hyperlink.Click -= ExecuteHyperLink;
			hyperlink.Click += ExecuteHyperLink;
		}

		private static void ExecuteHyperLink(Hyperlink sender, HyperlinkClickEventArgs args)
		{
			var command = GetCommand(sender);
			var parameter = GetCommandParameter(sender);

			if (command != null && command.CanExecute(parameter))
			{
				command.Execute(parameter);
			}
		}
	}
}
#endif
