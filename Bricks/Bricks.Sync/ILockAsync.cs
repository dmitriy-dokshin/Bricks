#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Sync
{
	/// <summary>
	/// Represents a non-blocking lock that is used for exclusive access to a resource.
	/// </summary>
	public interface ILockAsync
	{
		/// <summary>
		/// Acquires the lock.
		/// </summary>
		/// <returns>An <see cref="IDisposable" /> object that is used to release the lock.</returns>
		Task<IDisposable> Enter();

		/// <summary>
		/// Acquires the lock. Waiting for lock release could be cancelled through the <paramref name="cancellationToken" />.
		/// </summary>
		/// <param name="cancellationToken">
		/// The <see cref="CancellationToken" /> that is used to cancel waiting for lock release.
		/// </param>
		/// <returns>An <see cref="IDisposable" /> object that is used to release the lock.</returns>
		Task<IDisposable> Enter(CancellationToken cancellationToken);

		/// <summary>
		/// Attempts to acquire the lock.
		/// </summary>
		/// <param name="disposable">
		/// An <see cref="IDisposable" /> object that is used to release the lock. If the lock is acquired set <c>null</c>.
		/// </param>
		/// <returns>If the lock is not acquired returns <c>true</c>; otherwise, <c>false</c>.</returns>
		bool TryEnter(out IDisposable disposable);
	}
}