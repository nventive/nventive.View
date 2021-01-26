#if __IOS__
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Foundation;

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
			var timer = NSTimer.CreateScheduledTimer(dueTime, t =>
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

			disposable.Disposable = Disposable.Create(timer.Invalidate);
			NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Common);

			return disposable;
		}
	}
}
#endif
