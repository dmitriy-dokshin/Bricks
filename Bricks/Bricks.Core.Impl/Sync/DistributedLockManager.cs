#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.DateTime;
using Bricks.Core.Disposing;
using Bricks.Core.Extensions;
using Bricks.Core.Impl.Disposing;
using Bricks.Core.Repository;
using Bricks.Core.Results;
using Bricks.Core.Sync;

#endregion

namespace Bricks.Core.Impl.Sync
{
	internal sealed class DistributedLockManager : IDistributedLockManager
	{
		private static readonly object _defaultKey = string.Empty;
		private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan _defaultCheckPeriod = TimeSpan.FromSeconds(1);
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ILockContainer _lockContainer;

		public DistributedLockManager(IDateTimeProvider dateTimeProvider, ILockContainer lockContainer)
		{
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

		public async Task<IDisposable> TryGetLock(IRepository repository, object key, Guid ownerId, object key1 = null, TimeSpan? timeout = null)
		{
			if (!timeout.HasValue)
			{
				timeout = _defaultTimeout;
			}

			if (key == null)
			{
				key = _defaultKey;
			}

			if (key1 == null)
			{
				key1 = _defaultKey;
			}

			string keyString = key.ToString();
			string key1String = key1.ToStringOrNull();
			var @lock = await repository.Select<Lock>().FirstOrDefaultAsync(x => x.Key == keyString && x.Key1 == key1String, CancellationToken.None);
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
				IResult<Lock> addLockResult = await repository.AddAndSaveAsync(new Lock(keyString, key1String, _dateTimeProvider.Now, ownerId));
				if (addLockResult.Success)
				{
					lockDisposable = new LockDisposable(repository, addLockResult.Data);
				}
			}

			return lockDisposable;
		}

		public async Task<IDisposable> GetLock(IRepository repository, object key, Guid ownerId, object key1 = null, TimeSpan? timeout = null, TimeSpan? checkPeriod = null)
		{
			if (!timeout.HasValue)
			{
				timeout = _defaultTimeout;
			}

			if (key == null)
			{
				key = _defaultKey;
			}

			if (key1 == null)
			{
				key1 = _defaultKey;
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
					var ditributedDisposable = await TryGetLock(repository, key, ownerId, key1, timeout);
					if (ditributedDisposable != null)
					{
						return ditributedDisposable.After(localDisposable.Dispose);
					}

					await Task.Delay(checkPeriod.Value, CancellationToken.None);
				}
			}
		}

		public async Task<IResult> CleanUp(IRepository repository, TimeSpan lifetime)
		{
			DateTimeOffset createdAtFrom = _dateTimeProvider.Now - lifetime;
			IReadOnlyCollection<Lock> locks =
				await repository.Select<Lock>()
						  .Where(x => x.CreatedAt < createdAtFrom)
						  .ToReadOnlyCollectionAsync(CancellationToken.None);
			return await repository.RemoveRangeAndSaveAsync(locks);
		}

		#endregion
	}
}