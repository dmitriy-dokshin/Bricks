#region

using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Bricks.Core.IoC;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.WebAPI.Filters
{
	/// <summary>
	/// Поставщик фильтров на основе <see cref="ActionDescriptorFilterProvider" />, дополнительно инициализирующий экземпляры
	/// фильтров с исползованием <see cref="IServiceLocator" />.
	/// </summary>
	public sealed class ServiceLocatorFilterProvider : ActionDescriptorFilterProvider, IFilterProvider
	{
		private readonly IServiceLocator _serviceLocator;

		public ServiceLocatorFilterProvider(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}

		#region Implementation of IFilterProvider

		/// <summary>
		/// Returns an enumeration of filters.
		/// </summary>
		/// <returns>
		/// An enumeration of filters.
		/// </returns>
		/// <param name="configuration">The HTTP configuration.</param>
		/// <param name="actionDescriptor">The action descriptor.</param>
		IEnumerable<FilterInfo> IFilterProvider.GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
		{
			var filterInfos = GetFilters(configuration, actionDescriptor);
			foreach (var filterInfo in filterInfos)
			{
				_serviceLocator.BuildUp(filterInfo.Instance);
				yield return filterInfo;
			}
		}

		#endregion
	}
}