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
		/// <returns>Объект <see cref="IDisposable" />, который может использоваться для удаления подписки.</returns>
		IDisposable Subscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler, object sender = null)
			where TEventArgs : EventArgs;

		/// <summary>
		/// Подписывается на события с аргументами типа <typeparamref name="TEventArgs" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <typeparam name="TEventHandler">Тип обработчика события.</typeparam>
		/// <param name="sender">
		/// Отправитель события. Если не указан, обрабатываются все события с аргументами типа
		/// <typeparamref name="TEventArgs" />.
		/// </param>
		/// <returns>Объект <see cref="IDisposable" />, который может использоваться для удаления подписки.</returns>
		IDisposable Subscribe<TEventArgs, TEventHandler>(object sender = null)
			where TEventArgs : EventArgs
			where TEventHandler : IEventHandler<TEventArgs>;

		IDisposable Subscribe<TEventArgs>(out IObservable<TEventArgs> observable, EventHandlerObservableInvokeMode invokeMode = EventHandlerObservableInvokeMode.Series, object sender = null)
			where TEventArgs : EventArgs;

		/// <summary>
		/// Создаёт событие с аргументами <paramref name="args" /> от отправителя <paramref name="sender" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <param name="mode">Режим, в котором будут вызваны обработчики.</param>
		/// <returns>Задача обработки события.</returns>
		Task Raise<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken, RaiseMode mode = RaiseMode.Series)
			where TEventArgs : EventArgs;
	}
}