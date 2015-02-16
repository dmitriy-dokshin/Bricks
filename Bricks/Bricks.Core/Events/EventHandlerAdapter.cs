#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Events
{
	/// <summary>
	/// Адаптер <see cref="IEventHandler" /> для <see cref="EventHandler" />.
	/// </summary>
	public class EventHandlerAdapter : IEventHandler
	{
		private readonly EventHandler _eventHandler;

		public EventHandlerAdapter(EventHandler eventHandler)
		{
			_eventHandler = eventHandler;
		}

		#region Implementation of IEventHandler

		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public Task InvokeAsync(object sender, EventArgs args, CancellationToken cancellationToken)
		{
			return Task.Run(() => _eventHandler(sender, args), cancellationToken);
		}

		#endregion
	}

	/// <summary>
	/// Адаптер <see cref="IEventHandler{TArgs}" /> для <see cref="EventHandler{TEventArgs}" />.
	/// </summary>
	/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
	public sealed class EventHandlerAdapter<TEventArgs> : EventHandlerAdapter, IEventHandler<TEventArgs>
		where TEventArgs : EventArgs
	{
		private readonly EventHandler<TEventArgs> _eventHandler;

		public EventHandlerAdapter(EventHandler<TEventArgs> eventHandler)
			: base((sender, args) => eventHandler(sender, (TEventArgs)args))
		{
			_eventHandler = eventHandler;
		}

		#region Implementation of IEventHandler

		public Task InvokeAsync(object sender, TEventArgs args, CancellationToken cancellationToken)
		{
			return Task.Run(() => _eventHandler(sender, args), cancellationToken);
		}

		#endregion
	}
}