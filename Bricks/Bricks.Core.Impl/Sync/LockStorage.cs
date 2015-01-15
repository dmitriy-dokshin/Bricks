#region

using System;

using Bricks.Core.Collections;
using Bricks.Core.Sync;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Impl.Sync
{
	internal sealed class LockStorage : ILockStorage
	{
		private readonly IContainer<object, ILockContainer> _container;
		private readonly IServiceLocator _serviceLocator;

		public LockStorage(IContainerFactory containerFactory, IServiceLocator serviceLocator)
		{
			_container = containerFactory.Create<object, ILockContainer>();
			_serviceLocator = serviceLocator;
		}

		#region Implementation of ILockStorage

		public IDisposable GetContainer(object key, out ILockContainer lockContainer)
		{
			return _container.GetOrAdd(key, () => _serviceLocator.GetInstance<ILockContainer>(), out lockContainer);
		}

		#endregion
	}
}