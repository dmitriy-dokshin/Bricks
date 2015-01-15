#region

using System;

#endregion

namespace Bricks.Core.Sync
{
	public interface ILockStorage
	{
		IDisposable GetContainer(object key, out ILockContainer lockContainer);
	}
}