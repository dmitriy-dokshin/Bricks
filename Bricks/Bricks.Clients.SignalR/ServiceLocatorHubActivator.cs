#region

using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Clients.SignalR
{
	internal sealed class ServiceLocatorHubActivator : IHubActivator
	{
		private readonly IServiceLocator _serviceLocator;

		public ServiceLocatorHubActivator(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}

		#region Implementation of IHubActivator

		public IHub Create(HubDescriptor descriptor)
		{
			return (IHub)_serviceLocator.GetInstance(descriptor.HubType);
		}

		#endregion
	}
}