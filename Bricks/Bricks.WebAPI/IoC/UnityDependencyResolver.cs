#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.WebAPI.IoC
{
	public sealed class UnityDependencyResolver : IDependencyResolver
	{
		private readonly IUnityContainer _unityContainer;

		public UnityDependencyResolver(IUnityContainer unityContainer)
		{
			_unityContainer = unityContainer;
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_unityContainer.Dispose();
		}

		#endregion

		public event EventHandler<IUnityContainer> InitializeChildContainer;

		private void OnInitializeChildContainer(IUnityContainer childContainer)
		{
			var handler = InitializeChildContainer;
			if (handler != null)
			{
				handler(this, childContainer);
			}
		}

		#region Implementation of IDependencyScope

		/// <summary>
		/// Retrieves a service from the scope.
		/// </summary>
		/// <returns>
		/// The retrieved service.
		/// </returns>
		/// <param name="serviceType">The service to be retrieved.</param>
		public object GetService(Type serviceType)
		{
			try
			{
				var service = _unityContainer.Resolve(serviceType);
				return service;
			}
			catch (ResolutionFailedException)
			{
				// todo: добавить логирование
				return null;
			}
		}

		/// <summary>
		/// Retrieves a collection of services from the scope.
		/// </summary>
		/// <returns>
		/// The retrieved collection of services.
		/// </returns>
		/// <param name="serviceType">The collection of services to be retrieved.</param>
		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				return _unityContainer.ResolveAll(serviceType);
			}
			catch (ResolutionFailedException)
			{
				// todo: добавить логирование
				return Enumerable.Empty<object>();
			}
		}

		/// <summary>
		/// Starts a resolution scope.
		/// </summary>
		/// <returns>
		/// The dependency scope.
		/// </returns>
		public IDependencyScope BeginScope()
		{
			var childContainer = _unityContainer.CreateChildContainer();
			OnInitializeChildContainer(childContainer);
			return new UnityDependencyResolver(childContainer);
		}

		#endregion
	}
}