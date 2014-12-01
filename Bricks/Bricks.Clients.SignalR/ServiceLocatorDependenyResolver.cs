#region

using System;
using System.Collections.Generic;

using Bricks.Core.Exceptions;

using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Clients.SignalR
{
	internal sealed class ServiceLocatorDependenyResolver : DefaultDependencyResolver
	{
		private readonly IExceptionHelper _exceptionHelper;
		private readonly IServiceLocator _serviceLocator;

		public ServiceLocatorDependenyResolver(IServiceLocator serviceLocator, IExceptionHelper exceptionHelper)
		{
			_serviceLocator = serviceLocator;
			_exceptionHelper = exceptionHelper;
		}

		#region Implementation of IDependencyResolver

		public override object GetService(Type serviceType)
		{
			return base.GetService(serviceType)
				   ?? _exceptionHelper.SimpleCatch<object, ActivationException>(() => _serviceLocator.GetInstance(serviceType));
		}

		public override IEnumerable<object> GetServices(Type serviceType)
		{
			return base.GetServices(serviceType)
				   ?? _exceptionHelper.SimpleCatch<IEnumerable<object>, ActivationException>(() => _serviceLocator.GetAllInstances(serviceType));
		}

		#endregion
	}
}