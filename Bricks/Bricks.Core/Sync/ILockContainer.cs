#region

using System;

#endregion

namespace Bricks.Core.Sync
{
	public interface ILockContainer
	{
		IDisposable GetLock(object key, out ILockAsync @lock);
	}
}