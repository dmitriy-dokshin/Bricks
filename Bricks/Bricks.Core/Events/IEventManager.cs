#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Events
{
	/// <summary>
	/// Менеджер событий.
	/// </summary>
	public interface IEventManager
	{
		/// <summary>
		/// Подписывается на события с аргументами типа <typeparamref name="TEventArgs" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <param name="eventHandler">Обработчик события.</param>
		/// <param name="sender">
		/// Отправитель события. Если не указан, обрабатываются все события с аргументами типа
		/// <typeparamref name="TEventArgs" />.
		/// </param>
		void Subscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler, object sender = null) where TEventArgs : EventArgs;

		/// <summary>
		/// Подписывается на события с аргументами типа <typeparamref name="TEventArgs" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <typeparam name="TEventHandler">Тип обработчика события.</typeparam>
		/// <param name="sender">
		/// Отправитель события. Если не указан, обрабатываются все события с аргументами типа
		/// <typeparamref name="TEventArgs" />.
		/// </param>
		void Subscribe<TEventArgs, TEventHandler>(object sender = null) where TEventArgs : EventArgs where TEventHandler : IEventHandler<TEventArgs>;

		/// <summary>
		/// Создаёт событие с аргументами <paramref name="args" /> от отправителя <paramref name="sender" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		Task Raise<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken) where TEventArgs : EventArgs;
	}
}