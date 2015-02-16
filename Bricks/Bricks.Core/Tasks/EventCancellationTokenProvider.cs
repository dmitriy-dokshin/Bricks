#region

using System;
using System.Threading;

using Bricks.Core.Events;

#endregion

namespace Bricks.Core.Tasks
{
	public sealed class EventCancellationTokenProvider<TEventArgs> : ICancellationTokenProvider, IDisposable
		where TEventArgs : EventArgs
	{
		private readonly CancellationTokenSource _cancellationTokenSource;
		private readonly IDisposable _unsubscribeDisposable;

		public EventCancellationTokenProvider(IEventManager eventManager)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_unsubscribeDisposable = eventManager.Subscribe<TEventArgs>((sender, args) => _cancellationTokenSource.Cancel());
		}

		#region Implementation of ICancellationTokenProvider

		/// <summary>
		/// Получает токен отмены.
		/// </summary>
		/// <returns>Токен отмены.</returns>
		public CancellationToken GetCancellationToken()
		{
			return _cancellationTokenSource.Token;
		}

		#endregion

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_unsubscribeDisposable.Dispose();
		}

		#endregion
	}
}