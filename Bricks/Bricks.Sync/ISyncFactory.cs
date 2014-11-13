namespace Bricks.Sync
{
	/// <summary>
	/// Contains methods for creation of synchronization objects.
	/// </summary>
	public interface ISyncFactory
	{
		/// <summary>
		/// Creates new instance of <see cref="ILockAsync" /> type.
		/// </summary>
		/// <returns>New instance of <see cref="ILockAsync" /> type</returns>
		ILockAsync CreateAsyncLock(LockAsyncType type);
	}
}