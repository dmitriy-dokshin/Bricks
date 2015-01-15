#region

using System;
using System.Collections.Generic;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.Impl.IoC
{
	/// <summary>
	/// The implementation of <see cref="IServiceLocator" /> that uses <see cref="IUnityContainer" />
	/// and doesn't implement the  <see cref="IDisposable" />.
	/// </summary>
	internal sealed class SimpleUnityServiceLocator : ServiceLocatorImplBase
	{
		private readonly IUnityContainer _unityContainer;

		public SimpleUnityServiceLocator(IUnityContainer unityContainer)
		{
			_unityContainer = unityContainer;
		}

		#region Overrides of ServiceLocatorImplBase

		/// <summary>
		/// When implemented by inheriting classes, this method will do the actual work of resolving
		/// the requested service instance.
		/// </summary>
		/// <param name="serviceType">Type of instance requested.</param>
		/// <param name="key">Name of registered service you want. May be null.</param>
		/// <returns>
		/// The requested service instance.
		/// </returns>
		protected override object DoGetInstance(Type serviceType, string key)
		{
			return _unityContainer.Resolve(serviceType, key);
		}

		/// <summary>
		/// When implemented by inheriting classes, this method will do the actual work of
		/// resolving all the requested service instances.
		/// </summary>
		/// <param name="serviceType">Type of service requested.</param>
		/// <returns>
		/// Sequence of service instance objects.
		/// </returns>
		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return _unityContainer.ResolveAll(serviceType);
		}

		#endregion
	}
}