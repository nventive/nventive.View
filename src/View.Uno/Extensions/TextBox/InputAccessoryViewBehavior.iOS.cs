#if __IOS__
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using CoreGraphics;
using Uno.Extensions;
using UIKit;

namespace Nventive.View.Extensions
{
	/// <summary>
	/// This behavior adds an extendion on top of the keyboard for iOS. Each TextBox must be identified by a separate ID (string) in order for this behavior to work as intended.
	/// </summary>
	public class InputAccessoryViewBehavior
	{
		//The toolbar must be static to be usable in the Changed events.
		private static Toolbar _toolbar = new Toolbar();

		#region NEXT
		public static ICommand GetNextCommand(TextBox obj)
		{
			return (ICommand)obj.GetValue(NextCommandProperty);
		}

		public static void SetNextCommand(TextBox obj, ICommand value)
		{
			obj.SetValue(NextCommandProperty, value);
		}

		/// <summary>
		/// Default: Move to next focusable element
		/// </summary>
		public static readonly DependencyProperty NextCommandProperty =
			DependencyProperty.RegisterAttached("NextCommand", typeof(ICommand), typeof(InputAccessoryViewBehavior), new PropertyMetadata(new DelegateCommand(() => TryMoveToNext()), null));

		public static string GetNextLabel(TextBox obj)
		{
			return (string)obj.GetValue(NextLabelProperty);
		}

		public static void SetNextLabel(TextBox obj, string value)
		{
			obj.SetValue(NextLabelProperty, value);
		}

		public static readonly DependencyProperty NextLabelProperty =
			DependencyProperty.RegisterAttached("NextLabel", typeof(string), typeof(InputAccessoryViewBehavior), new PropertyMetadata(null, NextLabelChanged));

		private static void NextLabelChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			var box = (TextBox)sender;
			_toolbar.NextLabelChanged(box);
		}

		/// <summary>
		/// Executes the command for the Next button if it can.
		/// </summary>
		/// <param name="sender">The calling TextBox.</param>
		private static void ExecuteNextCommand(object sender)
		{
			var box = (TextBox)sender;

			if (box != null)
			{
				var cmd = GetNextCommand(box);
				const object param = null;
				if (cmd.CanExecute(param))
				{
					UpdateBinding(box);
					cmd.Execute(param);
				}
			}
		}

		/// <summary>
		/// Tries to move the focus to the next focusable element.
		/// </summary>
		/// <returns>If the focus has been moved</returns>
		private static bool TryMoveToNext()
		{
			return FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
		}
		#endregion

		#region PREVIOUS
		public static ICommand GetPreviousCommand(TextBox obj)
		{
			return (ICommand)obj.GetValue(PreviousCommandProperty);
		}

		public static void SetPreviousCommand(TextBox obj, ICommand value)
		{
			obj.SetValue(PreviousCommandProperty, value);
		}

		/// <summary>
		/// Default: Move to previous focusable element
		/// </summary>
		public static readonly DependencyProperty PreviousCommandProperty =
			DependencyProperty.RegisterAttached("PreviousCommand", typeof(ICommand), typeof(InputAccessoryViewBehavior), new PropertyMetadata(new DelegateCommand(() => TryMoveToPrevious()), null));

		public static string GetPreviousLabel(TextBox obj)
		{
			return (string)obj.GetValue(PreviousLabelProperty);
		}

		public static void SetPreviousLabel(TextBox obj, string value)
		{
			obj.SetValue(PreviousLabelProperty, value);
		}

		public static readonly DependencyProperty PreviousLabelProperty =
			DependencyProperty.RegisterAttached("PreviousLabel", typeof(string), typeof(InputAccessoryViewBehavior), new PropertyMetadata(null, PreviousLabelChanged));

		/// <summary>
		/// Tries to move the focus to the previous focusable element.
		/// </summary>
		/// <returns>If the focus has been moved</returns>
		private static bool TryMoveToPrevious()
		{
			return FocusManager.TryMoveFocus(FocusNavigationDirection.Previous);
		}

		private static void PreviousLabelChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			var box = (TextBox)sender;
			_toolbar.PreviousLabelChanged(box);
		}

		/// <summary>
		/// Executes the command for the Previous button if it can.
		/// </summary>
		/// <param name="sender">The calling TextBox.</param>
		private static void ExecutePreviousCommand(object sender)
		{
			var box = (TextBox)sender;

			if (box != null)
			{
				var cmd = GetPreviousCommand(box);
				const object param = null;
				if (cmd.CanExecute(param))
				{
					UpdateBinding(box);
					cmd.Execute(param);
				}
			}
		}
		#endregion

		#region DONE
		public static ICommand GetDoneCommand(TextBox obj)
		{
			return (ICommand)obj.GetValue(DoneCommandProperty);
		}

		public static void SetDoneDommand(TextBox obj, ICommand value)
		{
			obj.SetValue(DoneCommandProperty, value);
		}

		/// <summary>
		/// The default is set to DoNothing so when it goes into the ExecuteDoneCommand, it will close the keyboard and not throw an error.
		/// </summary>
		public static readonly DependencyProperty DoneCommandProperty =
			DependencyProperty.RegisterAttached("DoneCommand", typeof(ICommand), typeof(InputAccessoryViewBehavior), new PropertyMetadata(new DelegateCommand(() => DoNothing()), null));

		public static string GetDoneLabel(TextBox obj)
		{
			return (string)obj.GetValue(DoneLabelProperty);
		}

		public static void SetDoneLabel(TextBox obj, string value)
		{
			obj.SetValue(DoneLabelProperty, value);
		}

		public static readonly DependencyProperty DoneLabelProperty =
			DependencyProperty.RegisterAttached("DoneLabel", typeof(string), typeof(InputAccessoryViewBehavior), new PropertyMetadata(UIBarButtonSystemItem.Done, DoneLabelChanged));

		private static void DoneLabelChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			var box = (TextBox)sender;
			_toolbar.DoneLabelChanged(box);
		}

		/// <summary>
		/// Nothing should be done as default command of Done except closing the keyboard.
		/// </summary>
		private static void DoNothing()
		{
		}

		/// <summary>
		/// Executes the command for the Done button if it can, then close the keyboard.
		/// </summary>
		/// <param name="sender">The calling TextBox.</param>
		private static void ExecuteDoneCommand(object sender)
		{
			var box = (TextBox)sender;
			var textBox = new TextBoxWrapper(box);

			if (box != null)
			{
				var cmd = GetDoneCommand(box);
				const object param = null;
				if (cmd.CanExecute(param))
				{
					UpdateBinding(box);
					cmd.Execute(param);
				}
			}

			textBox.ResignFirstResponder();
		}
		#endregion

		/// <summary>
		/// Each TextBox needs an ID for the behavior to identify properly which textbox a button belongs to.
		/// </summary>
		#region ID
		public static string GetID(TextBox obj)
		{
			return (string)obj.GetValue(IDProperty);
		}

		public static void SetID(TextBox obj, string value)
		{
			obj.SetValue(IDProperty, value);
		}

		public static readonly DependencyProperty IDProperty =
			DependencyProperty.RegisterAttached("ID", typeof(string), typeof(InputAccessoryViewBehavior), new PropertyMetadata(null));
		#endregion

		private static void UpdateBinding(TextBox textBox)
		{
			textBox
				.GetBindingExpression(TextBox.TextProperty)?
				.UpdateSource(textBox.Text);
		}

		#region TOOLBAR
		/// <summary>
		/// Creates an instance of a toolbar. Since it is instanciated as static and is therefore the same for each textbox, 
		/// it keeps track of the buttons for each textbox in lists of value pairs (button, TextBox iD).
		/// </summary>
		private class Toolbar
		{
			private List<ValueTuple<UIBarButtonItem, string>> _previous = new List<ValueTuple<UIBarButtonItem, string>>();
			private List<ValueTuple<UIBarButtonItem, string>> _next = new List<ValueTuple<UIBarButtonItem, string>>();
			private List<ValueTuple<UIBarButtonItem, string>> _done = new List<ValueTuple<UIBarButtonItem, string>>();

			private UIBarButtonItem _flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

			public Toolbar()
			{
			}

			/// <summary>
			/// When the NextLabel is changed, it changes the button on the toolbar.
			/// </summary>
			/// <param name="box">The calling TextBox</param>
			public void NextLabelChanged(TextBox box)
			{
				var nextLbl = GetNextLabel(box);
				var iD = GetID(box);
				//If the value has been changed in the XAML, the iD might not be there yet.
				if (iD == null)
				{
					box.GotFocus += Box_GotFocusNext;
					return;
				}

				var nextButton = new UIBarButtonItem(nextLbl, UIBarButtonItemStyle.Plain, (s, e) => ExecuteNextCommand(box));

				//Searches to see if the button already exists. If yes, it changes the button to the new one,
				//if not, it adds it to the list.
				int i = 0;
				bool notFound = true;
				while (i < _next.Count && notFound)
				{
					var valueTuple = _next[i];
					if (string.Compare(valueTuple.Item2, iD) == 0)
					{
						valueTuple.Item1 = nextButton;
						_next[i] = valueTuple;
						notFound = false;
					}
					i++;
				}

				if (notFound)
				{
					_next.Add(new ValueTuple<UIBarButtonItem, string>(nextButton, iD));
				}

				SetToolBar(box);
			}

			/// <summary>
			/// The TextBox got focus after the NextLabel has been changed in the XAML
			/// </summary>
			/// <param name="sender">The calling TextBox</param>
			/// <param name="e"></param>
			private void Box_GotFocusNext(object sender, RoutedEventArgs e)
			{
				var box = (TextBox)sender;
				NextLabelChanged(box);
			}

			/// <summary>
			/// When the PreviousLabel is changed, it changes the button on the toolbar
			/// </summary>
			/// <param name="box">The calling TextBox</param>
			public void PreviousLabelChanged(TextBox box)
			{
				var previousLbl = GetPreviousLabel(box);
				var iD = GetID(box);
				//If the value has been changed in the XAML, the iD might not be there yet.
				if (iD == null)
				{
					box.GotFocus += Box_GotFocusPrevious;
					return;
				}

				var previousButton = new UIBarButtonItem(previousLbl, UIBarButtonItemStyle.Plain, (s, e) => ExecutePreviousCommand(box));

				//Searches to see if the button already exists. If yes, it changes the button to the new one,
				//if not, it adds it to the list.
				int i = 0;
				bool notFound = true;
				while (i < _previous.Count && notFound)
				{
					var valueTuple = _previous[i];
					if (string.Compare(valueTuple.Item2, iD) == 0)
					{
						valueTuple.Item1 = previousButton;
						_previous[i] = valueTuple;
						notFound = false;
					}
					i++;
				}

				if (notFound)
				{
					_previous.Add(new ValueTuple<UIBarButtonItem, string>(previousButton, iD));
				}

				SetToolBar(box);
			}

			/// <summary>
			/// The TextBox got focus after the PreviousLabel has been changed in the XAML
			/// </summary>
			/// <param name="sender">The calling TextBox</param>
			/// <param name="e"></param>
			private void Box_GotFocusPrevious(object sender, RoutedEventArgs e)
			{
				var box = (TextBox)sender;
				PreviousLabelChanged(box);
			}

			/// <summary>
			/// When the DoneLabel changes, it changes the button on the toolbar
			/// </summary>
			/// <param name="box">The calling TextBox</param>
			public void DoneLabelChanged(TextBox box)
			{
				var doneLbl = GetDoneLabel(box);
				var iD = GetID(box);
				//If the value has been changed in the XAML, the iD might not be there yet.
				if (iD == null)
				{
					box.GotFocus += Box_GotFocusDone;
					return;
				}

				var doneButton = new UIBarButtonItem(doneLbl, UIBarButtonItemStyle.Done, (s, e) => ExecuteDoneCommand(box));

				//Searches to see if the button already exists. If yes, it changes the button to the new one,
				//if not, it adds it to the list.
				int i = 0;
				bool notFound = true;
				while (i < _done.Count && notFound)
				{
					var valueTuple = _done[i];
					if (string.Compare(valueTuple.Item2, iD) == 0)
					{
						valueTuple.Item1 = doneButton;
						_done[i] = valueTuple;
						notFound = false;
					}
					i++;
				}

				if (notFound)
				{
					_done.Add(new ValueTuple<UIBarButtonItem, string>(doneButton, iD));
				}

				SetToolBar(box);
			}

			/// <summary>
			/// The TextBox got focus after the DoneLabel as been changed in the XAML
			/// </summary>
			/// <param name="sender">The calling TextBox</param>
			/// <param name="e"></param>
			private void Box_GotFocusDone(object sender, RoutedEventArgs e)
			{
				var box = (TextBox)sender;
				DoneLabelChanged(box);
			}

			/// <summary>
			/// Updates the toolbar on top of the keyboard with the right buttons.
			/// </summary>
			/// <param name="box">The calling TextBox</param>
			private void SetToolBar(TextBox box)
			{
				var textBox = new TextBoxWrapper(box);

				//The textbox hasn't been drawn yet
				if (!textBox.HasNativeTextBox())
				{
					return;
				}

				var toolbar = new UIToolbar(new CGRect(0, 0, box.Frame.Size.Width, 44f));
				var items = new List<UIBarButtonItem>();
				string iD = GetID(box);

				var previous = GetButton(iD, _previous);
				if (previous != null)
				{
					items.Add(previous);
				}

				var next = GetButton(iD, _next);
				if (next != null)
				{
					items.Add(next);
				}

				items.Add(_flexibleSpace);

				var done = GetButton(iD, _done);
				if (done != null)
				{
					items.Add(done);
				}

				toolbar.Items = items.ToArray();
				textBox.InputAccessoryView = toolbar;
			}

			/// <summary>
			/// Returns the first button in the list attached to the iD provided.
			/// </summary>
			/// <param name="iD">The TextBox ID</param>
			/// <param name="list">The Button/TextBox ID value pair list go through</param>
			/// <returns>The button matching the ID provided</returns>
			private UIBarButtonItem GetButton(string iD, List<ValueTuple<UIBarButtonItem, string>> list)
			{
				foreach (var valueTuple in list)
				{
					if (string.Compare(valueTuple.Item2, iD) == 0)
					{
						return valueTuple.Item1;
					}
				}

				//The button wasn't found.
				return null;
			}
		}
		#endregion

		#region TextBoxWrapper
		private class TextBoxWrapper
		{
			private readonly ITextBoxView _nativeTextBox;
			private readonly UITextField _textField;
			private readonly UITextView _textView;

			public TextBoxWrapper(TextBox box)
			{
				if (box != null)
				{
					_nativeTextBox = UIKit.UIViewExtensions.FindFirstChild<UITextField>(box) as ITextBoxView;

					if (_nativeTextBox == null)
					{
						_nativeTextBox = UIKit.UIViewExtensions.FindFirstChild<UITextView>(box) as ITextBoxView;
					}
				}
				
				_textView = _nativeTextBox as UITextView;
				_textField = _nativeTextBox as UITextField;

				_textField.Maybe(field => field.EditingDidBegin += (s, e) =>
				{
					_textField.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
				});
			}

			public void ResignFirstResponder() => _nativeTextBox?.ResignFirstResponder();

			public bool HasNativeTextBox() => _nativeTextBox != null;

			//InputAccessoryView is readonly by default in UIResponder but both UITextView and UITextField have a read-write version
			//In Uno, depending on if the TextBox is multiline or not, the TextBox will contain either a UITextView or a UITextField which
			//both implement ITextBoxView. This is just a wrapper to make life easier when accessing the InputAccessoryView. 
			public UIView InputAccessoryView
			{
				get
				{
					return _textField?.InputAccessoryView ?? _textView?.InputAccessoryView;
				}
				set
				{
					if (_textView != null)
					{
						_textView.InputAccessoryView = value;
						_textView.ReloadInputViews();
					}
					else if (_textField != null)
					{
						_textField.InputAccessoryView = value;
						_textField.ReloadInputViews();
					}

				}
			}
		}
		#endregion

		private class DelegateCommand : ICommand
		{
			private Action _executeMethod { get; }

			public DelegateCommand(Action executeMethod)
			{
				_executeMethod = executeMethod;
			}

			public event EventHandler CanExecuteChanged
			{
				add { }
				remove { }
			}

			public bool CanExecute(object parameter) => true;

			public void Execute(object parameter)
			{
				_executeMethod();
			}
		}
	}
}
#endif
