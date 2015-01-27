#region

using System;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Sync
{
	public interface IDistributedLockManager
	{
		Task<IDisposable> TryGetLock(Guid key, Guid ownerId, TimeSpan? timeout = null);

		Task<IDisposable> GetLock(Guid key, Guid ownerId, TimeSpan? timeout = null, TimeSpan? checkPeriod = null);
	}
}