#region

using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.EF.Sync
{
	public interface IDistributedLockManager
	{
		Task<IDisposable> TryGetLock<TDbContext>(Func<TDbContext> getDbContext, CancellationToken cancellationToken, object key, object key1 = null, TimeSpan? timeout = null)
			where TDbContext : DbContext;

		Task<IDisposable> GetLock<TDbContext>(
			Func<TDbContext> getDbContext, CancellationToken cancellationToken, object key, object key1 = null, TimeSpan? timeout = null, TimeSpan? checkPeriod = null)
			where TDbContext : DbContext;
	}
}