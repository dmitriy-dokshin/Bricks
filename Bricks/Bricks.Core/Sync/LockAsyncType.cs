namespace Bricks.Core.Sync
{
	/// <summary>
	/// Specifies various types of <see cref="ILockAsync" />.
	/// </summary>
	public enum LockAsyncType
	{
		/// <summary>
		/// Waiters will be released in random order.
		/// </summary>
		Random = 0,

		/// <summary>
		/// Waiters will be released in order of lock acquiring.
		/// </summary>
		Queue = 1,

		/// <summary>
		/// Waiters will be released in descending order of lock acquiring.
		/// </summary>
		Stack = 2
	}
}