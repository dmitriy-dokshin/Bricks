#region

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Bricks.Core.Reflection;
using Bricks.Core.Sync;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

#endregion

namespace Bricks.WebAPI.Filters
{
	public sealed class CacheFilter : IActionFilter
	{
		private IImmutableDictionary<string, ICacheManager> _cacheMangers;
		private readonly IInterlockedHelper _interlockedHelper;
		private readonly ILockStorage _lockStorage;
		private readonly IReflectionHelper _reflectionHelper;

		public CacheFilter(IInterlockedHelper interlockedHelper, ILockStorage lockStorage, IReflectionHelper reflectionHelper)
		{
			_interlockedHelper = interlockedHelper;
			_lockStorage = lockStorage;
			_reflectionHelper = reflectionHelper;

			_cacheMangers = ImmutableDictionary.Create<string, ICacheManager>();
		}

		#region Implementation of IFilter

		/// <summary>
		/// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single
		/// program element.
		/// </summary>
		/// <returns>
		/// true if more than one instance is allowed to be specified; otherwise, false. The default is false.
		/// </returns>
		public bool AllowMultiple
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Executes the filter action asynchronously.
		/// </summary>
		/// <returns>
		/// The newly created task for this operation.
		/// </returns>
		/// <param name="actionContext">The action context.</param>
		/// <param name="cancellationToken">The cancellation token assigned for this task.</param>
		/// <param name="continuation">The delegate function to continue after the action method is invoked.</param>
		public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
		{
			if (actionContext.Request.Method == HttpMethod.Get)
			{
				var cacheAttribute = actionContext.ActionDescriptor.GetCustomAttributes<CacheAttribute>().FirstOrDefault();
				if (cacheAttribute != null)
				{
					if (cacheAttribute.ServerLifetime.HasValue)
					{
						var cacheManagerKey = actionContext.ControllerContext.ControllerDescriptor.ControllerName + "." + actionContext.ActionDescriptor.ActionName;
						var cacheManager = _interlockedHelper.CompareExchange(ref _cacheMangers, x =>
							{
								ICacheManager result;
								IImmutableDictionary<string, ICacheManager> newValue;
								if (!x.TryGetValue(cacheManagerKey, out result))
								{
									result = CacheFactory.GetCacheManager(cacheManagerKey);
									newValue = x.Add(cacheManagerKey, result);
								}
								else
								{
									newValue = x;
								}

								return _interlockedHelper.CreateChangeResult(newValue, result);
							});

						var key = actionContext.Request.RequestUri.PathAndQuery;
						var httpResponseMessageData = (HttpResponseMessageData)cacheManager.GetData(key);
						if (httpResponseMessageData == null)
						{
							ILockContainer lockContainer;
							using (_lockStorage.GetContainer(_reflectionHelper.GetFullName(MethodBase.GetCurrentMethod()), out lockContainer))
							{
								ILockAsync @lock;
								using (lockContainer.GetLock(cacheManager, out @lock))
								{
									using (await @lock.Enter(cancellationToken))
									{
										httpResponseMessageData = (HttpResponseMessageData)cacheManager.GetData(key);
										if (httpResponseMessageData == null)
										{
											var httpResponseMessage = await continuation();
											if (cacheAttribute.ClientLifetime.HasValue)
											{
												var cacheControlHeaderValue = httpResponseMessage.Headers.CacheControl;
												cacheControlHeaderValue.Private = true;
												cacheControlHeaderValue.MaxAge = cacheAttribute.ClientLifetime.Value;
											}

											httpResponseMessageData = await HttpResponseMessageData.Create(httpResponseMessage);
											cacheManager.Add(key, httpResponseMessageData, CacheItemPriority.Normal, null, new AbsoluteTime(cacheAttribute.ServerLifetime.Value));
										}
									}
								}
							}
						}

						return httpResponseMessageData.ToResponseMessage();
					}

					if (cacheAttribute.ClientLifetime.HasValue)
					{
						var httpResponseMessage = await continuation();
						var cacheControlHeaderValue = httpResponseMessage.Headers.CacheControl;
						cacheControlHeaderValue.Private = true;
						cacheControlHeaderValue.MaxAge = cacheAttribute.ClientLifetime.Value;
						return httpResponseMessage;
					}
				}
			}

			return await continuation();
		}

		#endregion
	}
}