#region

using System;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.DateTime;
using Bricks.Core.Disposing;
using Bricks.Core.Impl.Disposing;
using Bricks.Core.Repository;
using Bricks.Core.Results;
using Bricks.Core.Sync;
using Bricks.Core.Tasks;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Impl.Sync
{
	internal sealed class DistributedLockManager : IDistributedLockManager
	{
		private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan _defaultCheckPeriod = TimeSpan.FromSeconds(1);
		private readonly CancellationToken _cancellationToken;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ILockContainer _lockContainer;
		private readonly IServiceLocator _serviceLocator;

		public DistributedLockManager(IServiceLocator serviceLocator, ICancellationTokenProvider cancellationTokenProvider, IDateTimeProvider dateTimeProvider, ILockContainer lockContainer)
		{
			_serviceLocator = serviceLocator;
			_cancellationToken = cancellationTokenProvider.GetCancellationToken();
			_dateTimeProvider = dateTimeProvider;
			_lockContainer = lockContainer;
		}

		private sealed class LockDisposable : DisposableBase
		{
			private readonly Lock _lock;
			private readonly IRepository _repository;

			public LockDisposable(IRepository repository, Lock @lock)
			{
				_repository = repository;
				_lock = @lock;
			}

			#region Overrides of DisposableBase

			protected override void Dispose(bool disposing)
			{
				if (IsDisposed)
				{
					return;
				}

				_repository.RemoveAndSaveAsync(_lock).Wait();

				base.Dispose(disposing);
			}

			#endregion
		}

		#region Implementation of IDistributedLockManager

		public async Task<IDisposable> TryGetLock(Guid key, Guid ownerId, TimeSpan? timeout = null)
		{
			if (!timeout.HasValue)
			{
				timeout = _defaultTimeout;
			}

			var repository = _serviceLocator.GetInstance<IRepository>();
			var @lock = await repository.Select<Lock>().FirstOrDefaultAsync(x => x.Key == key, _cancellationToken);
			if (@lock != null)
			{
				if (_dateTimeProvider.Now - @lock.CreatedAt > timeout.Value)
				{
					IResult removeLockResult = await repository.RemoveAndSaveAsync(@lock);
					if (removeLockResult.Success)
					{
						@lock = null;
					}
				}
			}

			IDisposable lockDisposable = null;
			if (@lock == null)
			{
				IResult<Lock> addLockResult = await repository.AddAndSaveAsync(new Lock(key, _dateTimeProvider.Now, ownerId));
				if (addLockResult.Success)
				{
					lockDisposable = new LockDisposable(repository, addLockResult.Data);
				}
			}

			return lockDisposable;
		}

		public async Task<IDisposable> GetLock(Guid key, Guid ownerId, TimeSpan? timeout = null, TimeSpan? checkPeriod = null)
		{
			if (!timeout.HasValue)
			{
				timeout = _defaultTimeout;
			}

			if (!checkPeriod.HasValue)
			{
				checkPeriod = _defaultCheckPeriod;
			}

			ILockAsync @lockAsync;
			using (_lockContainer.GetLock(key, out @lockAsync))
			{
				var cancellationTokenSource = new CancellationTokenSource(timeout.Value);
				var localDisposable = await @lockAsync.Enter(cancellationTokenSource.Token);
				while (true)
				{
					var ditributedDisposable = await TryGetLock(key, ownerId, timeout);
					if (ditributedDisposable != null)
					{
						return ditributedDisposable.After(localDisposable.Dispose);
					}

					await Task.Delay(checkPeriod.Value, CancellationToken.None);
				}
			}
		}

		#endregion
	}
}