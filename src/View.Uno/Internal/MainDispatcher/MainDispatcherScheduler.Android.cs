#if __ANDROID__
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Android.OS;

namespace Nventive.View
{
	internal partial class MainDispatcherScheduler
	{
		private static readonly Lazy<Handler> _handler = new Lazy<Handler>(() => new Handler(Looper.MainLooper), true);

		private IDisposable ScheduleCore<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			if (dueTime <= TimeSpan.Zero)
			{
				return Schedule(state, action);
			}

			var disposable = new SerialDisposable();

			void CancellableAction()
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
			}

			_handler.Value.PostDelayed(CancellableAction, (long)dueTime.TotalMilliseconds);

			return disposable;
		}
	}
}
#endif
