#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Events.Implementation
{
	/// <summary>
	/// Адаптер <see cref="IEventHandler" /> для <see cref="EventHandler" />.
	/// </summary>
	public sealed class EventHandlerAdapter : IEventHandler
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
	/// <typeparam name="TArgs">Тип аргументов события.</typeparam>
	public sealed class EventHandlerAdapter<TArgs> : IEventHandler<TArgs>
		where TArgs : EventArgs
	{
		private readonly EventHandler<TArgs> _eventHandler;

		public EventHandlerAdapter(EventHandler<TArgs> eventHandler)
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
		public Task InvokeAsync(object sender, TArgs args, CancellationToken cancellationToken)
		{
			return Task.Run(() => _eventHandler(sender, args), cancellationToken);
		}

		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public Task InvokeAsync(object sender, EventArgs args, CancellationToken cancellationToken)
		{
			return InvokeAsync(sender, (TArgs)args, cancellationToken);
		}

		#endregion
	}
}