using System;

using Bricks.Sync;

namespace Bricks.Helpers.Sync
{
	public interface ILockContainer
	{
		IDisposable GetLock(object key, out ILockAsync @lock);
	}
}