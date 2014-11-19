#region

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Helpers.Collections.Implementation
{
	internal sealed class ContainerFactory : IContainerFactory
	{
		private readonly IServiceLocator _serviceLocator;

		public ContainerFactory(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}

		#region Implementation of IContainerFactory

		public IContainer<TKey, TValue> Create<TKey, TValue>()
		{
			return _serviceLocator.GetInstance<Container<TKey, TValue>>();
		}

		#endregion
	}
}