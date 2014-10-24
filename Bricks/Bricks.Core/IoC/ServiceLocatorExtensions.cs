#region

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.IoC
{
	/// <summary>
	/// Contains extension methods for <see cref="IServiceLocator" />.
	/// </summary>
	public static class ServiceLocatorExtensions
	{
		/// <summary>
		/// Performs injection on object <paramref name="existing" />.
		/// </summary>
		/// <typeparam name="T">Type of object to perform injection on.</typeparam>
		/// <param name="serviceLocator">The service locator.</param>
		/// <param name="existing">Instance to build up.</param>
		/// <param name="resolverOverrides">Any overrides for the Buildup.</param>
		/// <returns>
		/// The resulting object. By default, this will be <paramref name="existing" />, but
		/// container extensions may add things like automatic proxy creation which would
		/// cause this to return a different object (but still type compatible with <typeparamref name="T" />).
		/// </returns>
		public static T BuildUp<T>(this IServiceLocator serviceLocator, T existing, params ResolverOverride[] resolverOverrides)
		{
			var unityContainer = serviceLocator.GetInstance<IUnityContainer>();
			return (T)unityContainer.BuildUp(existing.GetType(), existing, resolverOverrides);
		}
	}
}