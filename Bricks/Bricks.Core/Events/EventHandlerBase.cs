#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Events
{
	public abstract class EventHandlerBase<TEventArgs> : IEventHandler<TEventArgs>
		where TEventArgs : EventArgs
	{
		#region Implementation of IEventHandler

		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public abstract Task InvokeAsync(object sender, TEventArgs args, CancellationToken cancellationToken);

		/// <summary>
		/// Обрабатывает событие от объекта <paramref name="sender" /> с аргументами <paramref name="args" />.
		/// </summary>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public Task InvokeAsync(object sender, EventArgs args, CancellationToken cancellationToken)
		{
			return InvokeAsync(sender, (TEventArgs)args, cancellationToken);
		}

		#endregion
	}
}