#region

using System;

#endregion

namespace Bricks.Helpers.Sync
{
	public interface ILockStorage
	{
		IDisposable GetContainer(object key, out ILockContainer lockContainer);
	}
}