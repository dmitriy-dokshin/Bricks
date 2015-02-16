#region

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Disposing;
using Bricks.Core.Sync;

#endregion

namespace Bricks.Core.Events
{
	public class EventHandlerObservable : IEventHandler, IObservable<EventArgs>
	{
		private IImmutableList<IObserver<EventArgs>> _observers;

		public EventHandlerObservable(IDisposableHelper disposableHelper, IInterlockedHelper interlockedHelper, EventHandlerObservableInvokeMode invokeMode)
		{
			DisposableHelper = disposableHelper;
			InterlockedHelper = interlockedHelper;
			InvokeMode = invokeMode;
			_observers = ImmutableList.Create<IObserver<EventArgs>>();
		}

		protected IDisposableHelper DisposableHelper { get; private set; }

		protected IInterlockedHelper InterlockedHelper { get; private set; }

		protected EventHandlerObservableInvokeMode InvokeMode { get; set; }

		#region Implementation of IEventHandler

		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public async Task InvokeAsync(object sender, EventArgs args, CancellationToken cancellationToken)
		{
			switch (InvokeMode)
			{
				case EventHandlerObservableInvokeMode.Series:
					foreach (var observer in _observers)
					{
						observer.OnNext(args);
					}
					break;
				case EventHandlerObservableInvokeMode.Parallel:
					Task[] tasks = _observers.Select(x => Task.Run(() => x.OnNext(args), cancellationToken)).ToArray();
					await Task.WhenAll(tasks);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion

		#region Implementation of IObservable<out EventArgs>

		/// <summary>
		/// Notifies the provider that an observer is to receive notifications.
		/// </summary>
		/// <returns>
		/// A reference to an interface that allows observers to stop receiving notifications before the provider has finished
		/// sending them.
		/// </returns>
		/// <param name="observer">The object that is to receive notifications.</param>
		public IDisposable Subscribe(IObserver<EventArgs> observer)
		{
			InterlockedHelper.CompareExchange(ref _observers, x => x.Add(observer));
			return DisposableHelper.Action(() => InterlockedHelper.CompareExchange(ref _observers, x => x.Remove(observer)));
		}

		#endregion
	}

	public sealed class EventHandlerObservable<TEventArgs> : EventHandlerObservable, IEventHandler<TEventArgs>, IObservable<TEventArgs>
		where TEventArgs : EventArgs
	{
		private IImmutableList<IObserver<TEventArgs>> _observers;

		public EventHandlerObservable(IDisposableHelper disposableHelper, IInterlockedHelper interlockedHelper, EventHandlerObservableInvokeMode invokeMode)
			: base(disposableHelper, interlockedHelper, invokeMode)
		{
			_observers = ImmutableList.Create<IObserver<TEventArgs>>();
		}

		#region Implementation of IEventHandler<in TEventArgs>

		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public async Task InvokeAsync(object sender, TEventArgs args, CancellationToken cancellationToken)
		{
			await base.InvokeAsync(sender, args, cancellationToken);
			switch (InvokeMode)
			{
				case EventHandlerObservableInvokeMode.Series:
					foreach (var observer in _observers)
					{
						observer.OnNext(args);
					}
					break;
				case EventHandlerObservableInvokeMode.Parallel:
					Task[] tasks = _observers.Select(x => Task.Run(() => x.OnNext(args), cancellationToken)).ToArray();
					await Task.WhenAll(tasks);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion

		#region Implementation of IObservable<out TEventArgs>

		/// <summary>
		/// Notifies the provider that an observer is to receive notifications.
		/// </summary>
		/// <returns>
		/// A reference to an interface that allows observers to stop receiving notifications before the provider has finished
		/// sending them.
		/// </returns>
		/// <param name="observer">The object that is to receive notifications.</param>
		public IDisposable Subscribe(IObserver<TEventArgs> observer)
		{
			InterlockedHelper.CompareExchange(ref _observers, x => x.Add(observer));
			return DisposableHelper.Action(() => InterlockedHelper.CompareExchange(ref _observers, x => x.Remove(observer)));
		}

		#endregion
	}
}