#if WINDOWS_UWP || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Uno.Extensions;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Uno;

namespace Nventive.View.DataTemplateSelectors
{
	public class TypesDataTemplateSelector : DataTemplateSelector, IList<MatchType>
	{
		private Func<MatchType[]> _sanitizedTypes;

		public TypesDataTemplateSelector()
		{
			_sanitizedTypes = Funcs.CreateMemoized(() => _inner
				.Select(mt => new MatchType()
				{
					DataTemplate = mt.DataTemplate,
					Types = mt
						.Types
						.Split(new[] { ';' })
						.Select(type => type.Split(new[] { '.' }).LastOrDefault())
						.ToArray()
						.JoinBy(";")
				})
				.ToArray()
			);
		}

		public DataTemplate DefaultTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			return SelectTemplateCore(item);
		}

		protected override DataTemplate SelectTemplateCore(object item)
		{
			if (item == null)
			{
				return DefaultTemplate;
			}

			var matching = _sanitizedTypes()
				.FirstOrDefault(match => match
					.Types
					.Split(new[] { ';' })
					.Select(type => type.Equals(GetName(item), StringComparison.OrdinalIgnoreCase))
					.FirstOrDefault()
				);

			return matching?.DataTemplate ?? DefaultTemplate;
		}

		private static string GetName(object item)
		{
			var name = item.GetType().Name;

			//we want the type without it's generic info
			name = name.Split(new[] { '`' })[0];

			return name;
		}

		#region IList
		private IList<MatchType> _inner = new List<MatchType>();

		public IEnumerator<MatchType> GetEnumerator()
		{
			return _inner.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_inner).GetEnumerator();
		}

		public void Add(MatchType item)
		{
			_inner.Add(item);
		}

		public void Clear()
		{
			_inner.Clear();
		}

		public bool Contains(MatchType item)
		{
			return _inner.Contains(item);
		}

		public void CopyTo(MatchType[] array, int arrayIndex)
		{
			_inner.CopyTo(array, arrayIndex);
		}

		public bool Remove(MatchType item)
		{
			return _inner.Remove(item);
		}

		public int Count
		{
			get { return _inner.Count; }
		}

		public bool IsReadOnly
		{
			get { return _inner.IsReadOnly; }
		}

		public int IndexOf(MatchType item)
		{
			return _inner.IndexOf(item);
		}

		public void Insert(int index, MatchType item)
		{
			_inner.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_inner.RemoveAt(index);
		}

		public MatchType this[int index]
		{
			get { return _inner[index]; }
			set { _inner[index] = value; }
		}
		#endregion
	}

	[ContentProperty(Name = "DataTemplate")]
	public class MatchType
	{
		public string Types { get; set; }
		public DataTemplate DataTemplate { get; set; }

	}
}
#endif
