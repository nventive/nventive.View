using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Uno.Extensions;
using Uno.Logging;
#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using Windows.UI.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace Nventive.View.Converters
{
	/// <summary>
	/// This converter is intended to partition the entire double universe in xaml.
	/// It allows set a list of <see cref="Partition"/> directly in its content.
	/// Those partitions are helpful to : 
	///  - match a discrete value. <see cref="DiscretePartition" />
	///  - match a certain range of values <see cref="IntervalPartition" />
	///  The converter also allows to set a PartitionStrategy when custom computation must be done over the bound value.
	/// </summary>
	/// <remarks>
	/// If many partitions are considered in range, the first matching partition set will convert the value.
	/// For obvious performance reasons, no validation is made with the provided partition sets.
	/// A default value can be provided, in which case it will act as a partition itself, matching any other element of the set.
	/// If both <see cref="Partitions"/> and a <see cref="PartitionStrategy"/> are provided, the <see cref="Partitions" /> have precedence.
	/// </remarks>
#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
	[ContentProperty(Name = nameof(Partitions))]
#else
	[ContentProperty(nameof(Partitions))]
#endif
	public class PartitionConverter : ConverterBase
	{
		public PartitionConverter()
		{
			Partitions = new List<Partition>();
		}

		public List<Partition> Partitions { get; set; }

		public PartitionStrategy PartitionStrategy { get; set; }

		public object DefaultValue { get; set; }

		protected override object Convert(object value, Type targetType, object parameter)
		{
			var convertedValue = default(object);

			int? nullableInt;
			double? nullableDouble;
			var doubleValue = 0d;

			if ((nullableInt = value as int?).HasValue)
			{
				convertedValue = ConvertCore(nullableInt.Value);
			}
			else if ((nullableDouble = value as double?).HasValue)
			{
				convertedValue = ConvertCore(nullableDouble.Value);
			}
			else if (double.TryParse(value?.ToString(), NumberStyles.Number, CultureInfo.CurrentUICulture, out doubleValue))
			{
				convertedValue = ConvertCore(doubleValue);
			}
			else
			{
				this.Log().DebugIfEnabled(() => $"The provided conversion value could not be converted to a double. Consider binding a numeric value when using a {typeof(PartitionConverter).Name}.");
			}

			return convertedValue ?? DefaultValue ?? value;
		}

		private object ConvertCore(double value)
		{
			return Partitions
				.FirstOrDefault(set => set.IsInRange(value))
				.SelectOrDefault(
					set => set.InRangeValue,
					PartitionStrategy?.Execute(value)
				);
		}
	}
}
