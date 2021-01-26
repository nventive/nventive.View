#if __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Windows.UI.Core;

namespace Nventive.View
{
	internal partial class MainDispatcherScheduler : IScheduler
	{
		private readonly CoreDispatcher _dispatcher;
		private readonly CoreDispatcherPriority _priority;

		public MainDispatcherScheduler(CoreDispatcher dispatcher)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			_priority = CoreDispatcherPriority.Normal;
		}

		public MainDispatcherScheduler(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			_priority = priority;
		}

		public DateTimeOffset Now => DateTimeOffset.Now;

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
		{
			var subscription = new SerialDisposable();
			var d = new CancellationDisposable();

			_dispatcher.RunAsync(
				_priority, () =>
				{
					if (!subscription.IsDisposed)
					{
						subscription.Disposable = action(this, state);
					}
				})
				.AsTask(d.Token);

			return new CompositeDisposable(subscription, d);
		}

		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action) =>
			ScheduleCore(state, dueTime - Now, action);

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action) =>
			ScheduleCore(state, dueTime, action);
	}
}
#endif
