using System;

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter outputs a custom value based on on the presence or absence of characters in a string.
	/// 
	/// ValueIfEmpty: the value that should be returned if the string empty.
	/// ValueIfNotEmpty: the value that shold be returned if the string is not empty
	/// 
	/// </summary>
	public class FromEmptyStringToCustomValueConverter : ConverterBase
	{
		public object ValueIfEmpty { get; set; }

		public object ValueIfNotEmpty { get; set; }

		protected override object Convert(object value, Type targetType, object parameter)
		{
			var str = value as string;

			if (string.IsNullOrEmpty(str))
			{
				return ValueIfEmpty;
			}
			else
			{
				return ValueIfNotEmpty;
			}
		}
	}
}
