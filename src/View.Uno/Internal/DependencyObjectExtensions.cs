using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uno.Extensions;
using System.Reactive.Linq;
#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using _DependencyObject = Windows.UI.Xaml.DependencyObject;
using _FrameworkElement = Windows.UI.Xaml.FrameworkElement;
#elif __ANDROID__ || __IOS__ || __WASM__
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using _DependencyObject = Windows.UI.Xaml.DependencyObject;
using _FrameworkElement = Windows.UI.Xaml.FrameworkElement;
#endif
#if WINDOWS_UWP || __WASM__
using _View = Windows.UI.Xaml.FrameworkElement;
#elif __ANDROID__
using _View = Android.Views.View;
#elif __IOS__
using _View = UIKit.UIView;
using UIKit;
#endif


namespace Nventive.View
{
	internal static partial class DependencyObjectExtensions
	{
		/// <summary>
		/// This is the class passed in the Rx IObservable when
		/// this method is used as notification mechanism on
		/// DependencyPropertyFactory.Register()
		/// </summary>
		/// <remarks>
		/// You can use the .NewValues() if you're only interested by
		/// the new values.
		/// </remarks>
		internal class DependencyPropertyChanged<T>
		{
			public DependencyPropertyChanged()
			{
				// the constructor is not public
			}

			public T OldValue { get; set; }
			public T NewValue { get; set; }
			public object Sender { get; set; }
		}

		/// <summary>
		/// Returns a never completing observable that will provide the new values for the specified property name on the specified FrameworkElement
		/// </summary>
		internal static IObservable<DependencyPropertyChanged<TValue>> ObservePropertyChanged<TValue>(this _DependencyObject instance, DependencyProperty property)
		{
#if __ANDROID__ || __IOS__ || __WASM__
			return Observable.Create<DependencyPropertyChanged<TValue>>(observer =>
			{
				return instance.RegisterDisposablePropertyChangedCallback(property, (sender, args) =>
				{
					observer.OnNext(new DependencyPropertyChanged<TValue>
					{
						OldValue = (TValue)args.OldValue,
						NewValue = (TValue)args.NewValue,
						Sender = sender
					});
				});
			});
#elif WINDOWS_UWP
			return Observable.Create<DependencyPropertyChanged<TValue>>(observer =>
			{
				var oldValue = (TValue)instance.GetValue(property);
				var handle = instance.RegisterPropertyChangedCallback(property, (sender, prop) =>
				{
					var newValue = (TValue)sender.GetValue(prop);
					var args = new DependencyPropertyChanged<TValue>
					{
						OldValue = oldValue,
						NewValue = newValue,
						Sender = sender
					};

					oldValue = newValue;

					observer.OnNext(args);
				});

				return () =>
				{
					instance.UnregisterPropertyChangedCallback(property, handle);
				};
			});
#endif
		}

		/// <summary>
		/// Traverses the Visual Tree upwards looking for the ancestor that satisfies the <paramref name="predicate"/>.
		/// </summary>
		/// <param name="dependencyObject">The element for which the ancestor is being looked for.</param>
		/// <param name="predicate">The predicate that evaluates if an element is the ancestor that is being looked for.</param>
		/// <returns>
		/// The ancestor element that matches the <paramref name="predicate"/> or <see langword="null"/>
		/// if the ancestor was not found.
		/// </returns>
		internal static _DependencyObject FindParent(this _DependencyObject dependencyObject, Func<_DependencyObject, bool> predicate)
		{
			if (predicate(dependencyObject))
			{
				return dependencyObject;
			}
			else
			{
				var parent = dependencyObject.FindParent();

				if (parent == null)
				{
					return null;
				}
				else
				{
					return FindParent(parent, predicate);
				}
			}
		}

		internal static _DependencyObject FindParent(this _DependencyObject dependencyObject)
		{
			_DependencyObject parent = null;

			_FrameworkElement frameworkElement = dependencyObject as _FrameworkElement;

			if (frameworkElement != null)
			{
				parent = frameworkElement.Parent ?? VisualTreeHelper.GetParent(frameworkElement);
			}

			return parent;
		}

		internal static IEnumerable<_DependencyObject> GetParentHierarchy(this _DependencyObject element, bool includeCurrent = true)
		{
			if (includeCurrent)
			{
				yield return element;
			}

			for (var parent = (element as _FrameworkElement).SelectOrDefault(e => e.Parent) ?? VisualTreeHelper.GetParent(element);
				 parent != null;
				 parent = VisualTreeHelper.GetParent(parent))
			{
				yield return parent;
			}
		}

		internal static T FindFirstParent<T>(this _DependencyObject element, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return element.GetParentHierarchy(includeCurrent).OfType<T>().FirstOrDefault();
		}

		internal static T FindFirstParent<T>(this _DependencyObject element, Func<T, bool> selector, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return element.GetParentHierarchy(includeCurrent).OfType<T>().FirstOrDefault(selector);
		}

		internal static T FindFirstChild<T>(this _DependencyObject element, int? childLevelLimit = null, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return element.FindFirstChild<T>(x => true, childLevelLimit, includeCurrent);
		}

		internal static T FindFirstChild<T>(this _DependencyObject element, Func<T, bool> selector, int? childLevelLimit = null, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return InnerFindFirstChild(new[] { element }.Trim(), selector, childLevelLimit, includeCurrent);
		}

		internal static T InnerFindFirstChild<T>(IEnumerable<_DependencyObject> elements, Func<T, bool> selector, int? childLevelLimit, bool includeCurrentLevel)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			if (elements.None() || (childLevelLimit.HasValue && childLevelLimit <= 0))
			{
				return null;
			}
			else if (includeCurrentLevel)
			{
				return elements.OfType<T>().FirstOrDefault(selector)
					?? InnerFindFirstChild(elements.SelectMany(GetChildren), selector, childLevelLimit.HasValue ? childLevelLimit - 1 : null, true);
			}
			else
			{
				return InnerFindFirstChild(elements.SelectMany(GetChildren), selector, childLevelLimit.HasValue ? childLevelLimit - 1 : null, true);
			}
		}

		/// <summary>
		/// Enumerates the children of the provided element, using the logical tree instead of the visual tree.
		/// </summary>
		internal static IEnumerable<T> FindAllLogicalChildren<T>(this _DependencyObject element, int? childLevelLimit = null, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return element.FindAllLogicalChildren<T>(x => true, childLevelLimit, includeCurrent);
		}

		/// <summary>
		/// Enumerates the children of the provided element, using the logical tree instead of the visual tree.
		/// </summary>
		internal static IEnumerable<T> FindAllLogicalChildren<T>(this _DependencyObject element, Func<T, bool> selector, int? childLevelLimit = null, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return InnerFindAllLogicalChildren<T>(element, selector, childLevelLimit, includeCurrent);
		}

		// Remark : Could be even more optimal to use full yield with no recursion (ie.: Stack)
		internal static IEnumerable<T> InnerFindAllLogicalChildren<T>(_DependencyObject reference, Func<T, bool> selector, int? childLevelLimit, bool includeCurrentLevel)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			IEnumerable<T> innerChildren = null;
			// should we check the current object?
			if (includeCurrentLevel)
			{
				var current = reference as T;
				// if object match desired type + selector
				if (current != null && selector(current))
				{
					innerChildren = Enumerable.Repeat(current, 1);
				}
			}

			// still have some more children to check?
			if (childLevelLimit.HasValue &&
				childLevelLimit <= 0)
			{
				return innerChildren ?? Enumerable.Empty<T>();
			}
			if (innerChildren == null)
			{
				innerChildren = Enumerable.Empty<T>();
			}

			var children = reference.GetLogicalChildrenInternal();

			// check how many children exist for current object, if no more, return what we found!
			if (children.None())
			{
				return innerChildren;
			}

			// get all the children of current object's children
			var otherChilds = children
				.SelectMany(child => InnerFindAllLogicalChildren(child, selector, childLevelLimit.HasValue ? childLevelLimit - 1 : null, true));

			return innerChildren.Concat(otherChilds);
		}

		internal static IEnumerable<_DependencyObject> GetLogicalChildrenInternal(this _DependencyObject reference)
		{
			// Try to determine the logical children of the current reference, using know control types.

			var panel = reference as Panel;
			if (panel != null)
			{
				foreach (var item in panel.Children.OfType<_DependencyObject>())
				{
					yield return item;
				}
			}

			var content = reference as ContentControl;
			if (content != null)
			{
				yield return content.Content as _DependencyObject;
			}

			var border = reference as Border;
			if (border != null)
			{
				yield return border.Child as _DependencyObject;
			}
		}

		internal static IEnumerable<T> FindAllChildren<T>(this _DependencyObject element, int? childLevelLimit = null, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return element.FindAllChildren<T>(x => true, childLevelLimit, includeCurrent);
		}

		internal static IEnumerable<T> FindAllChildren<T>(this _DependencyObject element, Func<T, bool> selector, int? childLevelLimit = null, bool includeCurrent = true)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			return InnerFindAllChildren<T>(element, selector, childLevelLimit, includeCurrent);
		}

		// Remark : Could be even more optimal to use full yield with no recursion (ie.: Stack)
		internal static IEnumerable<T> InnerFindAllChildren<T>(_DependencyObject reference, Func<T, bool> selector, int? childLevelLimit, bool includeCurrentLevel)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{

			IEnumerable<T> innerChildren = null;
			// should we check the current object?
			if (includeCurrentLevel)
			{
				var current = reference as T;
				// if object match desired type + selector
				if (current != null && selector(current))
				{
					innerChildren = Enumerable.Repeat(current, 1);
				}
			}
			// still have some more children to check?
			if (childLevelLimit.HasValue &&
				childLevelLimit <= 0)
			{
				return innerChildren ?? Enumerable.Empty<T>();
			}
			if (innerChildren == null)
			{
				innerChildren = Enumerable.Empty<T>();
			}
			// check how many children exist for current object, if no more, return what we found!
			var count = VisualTreeHelper.GetChildrenCount(reference);
			if (count == 0)
			{
				return innerChildren;
			}
			// get all the children of current object's children
			var otherChilds = reference
								.GetChildrenInternal(count)
								.SelectMany(child => InnerFindAllChildren(child, selector, childLevelLimit.HasValue ? childLevelLimit - 1 : null, true));
			return innerChildren.Concat(otherChilds);

		}

		internal static IEnumerable<_DependencyObject> GetChildrenInternal(this _DependencyObject reference, int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return VisualTreeHelper.GetChild(reference, i);
			}
		}

		internal static IEnumerable<_DependencyObject> GetChildren(this _DependencyObject obj)
		{
			var count = VisualTreeHelper.GetChildrenCount(obj);

			for (int i = 0; i < count; i++)
			{
				yield return VisualTreeHelper.GetChild(obj, i);
			}
		}

		internal static T GetChildElementByName<T>(this _DependencyObject parent, int maximumDepth, string name)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			if (parent == null ||
				maximumDepth == 0)
			{
				return null;
			}

			var toCheckList = new List<_DependencyObject>();
			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				var currentChild = VisualTreeHelper.GetChild(parent, i);
				if (currentChild is T)
				{
					if (currentChild is _FrameworkElement &&
						((_FrameworkElement)currentChild).Name == name)
					{
						return currentChild as T;
					}
				}
				toCheckList.Add(currentChild);
			}
			foreach (var item in toCheckList)
			{
				var resVal = item.GetChildElementByName<T>(maximumDepth - 1, name);
				if (resVal != null)
				{
					return resVal;
				}
			}
			return null;
		}

		internal static T GetFirstParentElement<T>(this _DependencyObject child)
			where T :
#if __ANDROID__ || __IOS__ || __WASM__
			class,
#endif
			_DependencyObject
		{
			if (child == null)
			{
				return null;
			}
			var parent = VisualTreeHelper.GetParent(child);
			if (parent is T)
			{
				return (T)parent;
			}
			return GetFirstParentElement<T>(parent);
		}
	}
}
