#region

using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Bricks.Core.Disposing;
using Bricks.Core.Exceptions;
using Bricks.Core.Results;
using Bricks.Core.Sync;
using Bricks.EF.Entities;
using Bricks.EF.Extensions;

#endregion

namespace Bricks.EF.Sync.Impl
{
	internal sealed class DistributedLockManager : IDistributedLockManager
	{
		private static readonly object _defaultKey = string.Empty;
		private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan _defaultCheckPeriod = TimeSpan.FromSeconds(1);
		private readonly IExceptionHelper _exceptionHelper;
		private readonly ILockContainer _lockContainer;

		public DistributedLockManager(ILockContainer lockContainer, IExceptionHelper exceptionHelper)
		{
			_lockContainer = lockContainer;
			_exceptionHelper = exceptionHelper;
		}

		private sealed class LockDisposable<TDbContext> : DisposableBase
			where TDbContext : DbContext
		{
			private readonly Func<TDbContext> _getRepository;
			private readonly Lock _lock;

			public LockDisposable(Func<TDbContext> getRepository, Lock @lock)
			{
				_getRepository = getRepository;
				_lock = @lock;
			}

			#region Overrides of DisposableBase

			protected override void Dispose(bool disposing)
			{
				if (IsDisposed)
				{
					return;
				}

				using (var dbContext = _getRepository())
				{
					dbContext.Remove(_lock);
					dbContext.SaveChanges();
				}

				base.Dispose(disposing);
			}

			#endregion
		}

		#region Implementation of IDistributedLockManager

		public async Task<IDisposable> TryGetLock<TDbContext>(Func<TDbContext> getDbContext, CancellationToken cancellationToken, object key, object key1 = null, TimeSpan? timeout = null)
			where TDbContext : DbContext
		{
			if (!timeout.HasValue)
			{
				timeout = _defaultTimeout;
			}

			if (key == null)
			{
				throw new ArgumentNullException("key");
			}

			if (key1 == null)
			{
				key1 = _defaultKey;
			}

			var keyString = key.ToString();
			var key1String = key1.ToString();
			using (var dbContext = getDbContext())
			{
				var @lock = await dbContext.Set<Lock>().FirstOrDefaultAsync(x => x.Key == keyString && x.Key1 == key1String, CancellationToken.None);
				if (@lock != null)
				{
					if (DateTimeOffset.Now - @lock.CreatedAt > timeout.Value)
					{
						IResult removeLockResult = await _exceptionHelper.CatchAsync(dbContext.Remove(@lock).SaveChangesAsync(cancellationToken));
						if (removeLockResult.Success)
						{
							@lock = null;
						}
					}
				}

				IDisposable lockDisposable = null;
				if (@lock == null)
				{
					@lock = new Lock(keyString, key1String, DateTime.UtcNow);
					var addLockResult = await _exceptionHelper.CatchAsync(dbContext.Add(@lock).SaveChangesAsync(cancellationToken));
					if (addLockResult.Success)
					{
						lockDisposable = new LockDisposable<TDbContext>(getDbContext, @lock);
					}
				}

				return lockDisposable;
			}
		}

		public async Task<IDisposable> GetLock<TDbContext>(Func<TDbContext> getDbContext, CancellationToken cancellationToken, object key, object key1 = null, TimeSpan? timeout = null, TimeSpan? checkPeriod = null) where TDbContext : DbContext
		{
			if (!timeout.HasValue)
			{
				timeout = _defaultTimeout;
			}

			if (key == null)
			{
				throw new ArgumentNullException("key");
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
					var ditributedDisposable = await TryGetLock(getDbContext, cancellationToken, key, key1, timeout);
					if (ditributedDisposable != null)
					{
						return ditributedDisposable.After(localDisposable.Dispose);
					}

					cancellationToken.ThrowIfCancellationRequested();
					await Task.Delay(checkPeriod.Value, CancellationToken.None);
				}
			}
		}

		#endregion
	}
}