#if NETSTANDARD2_0
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace Nventive.View
{
	internal partial class MainDispatcherScheduler
	{
		private IDisposable ScheduleCore<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			if (dueTime <= TimeSpan.Zero)
			{
				return Schedule(state, action);
			}

			var disposable = new SerialDisposable();
			var timer = new System.Threading.Timer(t =>
			{
				if (!disposable.IsDisposed)
				{
					try
					{
						disposable.Disposable = action(this, state);
					}
					catch (Exception)
					{
						disposable.Dispose();
					}
				}
			});

			timer.Change((int)dueTime.TotalMilliseconds, System.Threading.Timeout.Infinite);

			return disposable;
		}
	}
}
#endif
