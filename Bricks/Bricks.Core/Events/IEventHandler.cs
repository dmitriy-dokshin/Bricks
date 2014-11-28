#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Events
{
	/// <summary>
	/// Интерфейс обработчика событий.
	/// </summary>
	public interface IEventHandler
	{
		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		Task InvokeAsync(object sender, EventArgs args, CancellationToken cancellationToken);
	}

	/// <summary>
	/// Интерфейс обработчика событий с аргументами типа <typeparamref name="TArgs" />.
	/// </summary>
	/// <typeparam name="TArgs">Тип аргементов события.</typeparam>
	public interface IEventHandler<in TArgs> : IEventHandler
		where TArgs : EventArgs
	{
		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		Task InvokeAsync(object sender, TArgs args, CancellationToken cancellationToken);
	}

	/// <summary>
	/// Базовый класс для упрощения реализации <see cref="IEventHandler{TArgs}" />.
	/// </summary>
	/// <typeparam name="TArgs">Тип аргементов события.</typeparam>
	public abstract class EventHandlerBase<TArgs> : IEventHandler<TArgs>
		where TArgs : EventArgs
	{
		#region Implementation of IEventHandler

		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public abstract Task InvokeAsync(object sender, TArgs args, CancellationToken cancellationToken);

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