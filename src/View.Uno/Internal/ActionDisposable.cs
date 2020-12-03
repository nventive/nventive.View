using System;
using System.Collections.Generic;
using System.Text;

namespace Chinook.View
{
	internal class ActionDisposable : IDisposable
	{
		private readonly Action _action;

		public ActionDisposable(Action action)
		{
			_action = action;
		}

		public void Dispose()
		{
			_action?.Invoke();
		}
	}
}
