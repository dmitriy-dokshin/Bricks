#region

using System;

using Bricks.Helpers.Collections;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Helpers.Sync.Implementation
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