#region

using Bricks.Core.IoC;
using Bricks.Core.Sync;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Impl.Sync
{
	/// <summary>
	/// The default implementation of <see cref="ISyncFactory" />.
	/// </summary>
	public class SyncFactory : ISyncFactory
	{
		private readonly IServiceLocator _serviceLocator;

		public SyncFactory(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}

		#region Implementation of ISyncFactory

		/// <summary>
		/// Creates new instance of <see cref="ILockAsync" /> type.
		/// </summary>
		/// <returns>New instance of <see cref="ILockAsync" /> type</returns>
		public virtual ILockAsync CreateAsyncLock(LockAsyncType type = LockAsyncType.Queue)
		{
			return _serviceLocator.BuildUp(new LockAsync(type));
		}

		#endregion
	}
}