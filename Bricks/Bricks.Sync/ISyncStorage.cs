namespace Bricks.Sync
{
	public interface ISyncStorage
	{
		ILockAsync GetLock();
	}
}