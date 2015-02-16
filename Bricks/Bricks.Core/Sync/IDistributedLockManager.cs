#region

using System;
using System.Threading.Tasks;

using Bricks.Core.Repository;
using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Sync
{
	public interface IDistributedLockManager
	{
		Task<IDisposable> TryGetLock(IRepository repository, object key, Guid ownerId, object key1 = null, TimeSpan? timeout = null);

		Task<IDisposable> GetLock(IRepository repository, object key, Guid ownerId, object key1 = null, TimeSpan? timeout = null, TimeSpan? checkPeriod = null);

		Task<IResult> CleanUp(IRepository repository, TimeSpan lifetime);
	}
}