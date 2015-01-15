namespace Bricks.Core.Sync
{
	public interface ISyncStorage
	{
		ILockAsync GetLock();
	}
}