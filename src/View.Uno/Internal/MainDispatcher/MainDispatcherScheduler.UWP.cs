#if WINDOWS_UWP
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using Windows.UI.Core;

namespace Nventive.View
{
	internal partial class MainDispatcherScheduler : IScheduler
	{
		private readonly IScheduler _coreDispatcherScheduler;

		public MainDispatcherScheduler(CoreDispatcher dispatcher)
		{
			if (dispatcher == null)
			{
				throw new ArgumentNullException(nameof(dispatcher));
			}

			_coreDispatcherScheduler = new CoreDispatcherScheduler(dispatcher);
		}

		public MainDispatcherScheduler(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
		{
			if (dispatcher == null)
			{
				throw new ArgumentNullException(nameof(dispatcher));
			}

			_coreDispatcherScheduler = new CoreDispatcherScheduler(dispatcher, priority);
		}

		public DateTimeOffset Now => _coreDispatcherScheduler.Now;

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
			=> _coreDispatcherScheduler.Schedule(state, action);

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
			=> _coreDispatcherScheduler.Schedule(state, dueTime, action);

		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
			=> _coreDispatcherScheduler.Schedule(state, dueTime, action);
	}
}
#endif
