#region

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
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
		private readonly IInterlockedHelper _interlockedHelper;
		private readonly ILockStorage _lockStorage;
		private readonly IReflectionHelper _reflectionHelper;
		private readonly IWebCacheController _webCacheController;
		private IImmutableDictionary<string, ICacheManager> _cacheMangers;

		public CacheFilter(IInterlockedHelper interlockedHelper, ILockStorage lockStorage, IReflectionHelper reflectionHelper, IWebCacheController webCacheController)
		{
			_interlockedHelper = interlockedHelper;
			_lockStorage = lockStorage;
			_reflectionHelper = reflectionHelper;
			_webCacheController = webCacheController;

			_cacheMangers = ImmutableDictionary.Create<string, ICacheManager>();
		}

		private static void SetClentCacheControl(HttpResponseMessage httpResponseMessage, CacheAttribute cacheAttribute)
		{
			CacheControlHeaderValue cacheControl = httpResponseMessage.Headers.CacheControl;
			if (cacheControl == null)
			{
				cacheControl = new CacheControlHeaderValue();
				httpResponseMessage.Headers.CacheControl = cacheControl;
			}

			cacheControl.Private = true;
			cacheControl.MaxAge = TimeSpan.FromSeconds(cacheAttribute.ClientLifetime);
		}

		private static string GetKey(HttpRequestMessage request, IReadOnlyCollection<string> headers)
		{
			string pathAndQuery = request.RequestUri.PathAndQuery;
			bool hasParams = pathAndQuery.IndexOf('?') >= 0;
			var keyBuilder = new StringBuilder(pathAndQuery);
			if (headers != null && headers.Count > 0)
			{
				foreach (string header in headers)
				{
					IEnumerable<string> values;
					if (request.Headers.TryGetValues(header, out values))
					{
						foreach (string value in values)
						{
							if (!hasParams)
							{
								keyBuilder.Append('?');
								hasParams = true;
							}
							else
							{
								keyBuilder.Append('&');
							}

							keyBuilder.Append(header);
							keyBuilder.Append('=');
							keyBuilder.Append(Uri.EscapeDataString(value));
						}
					}
				}
			}

			string key = keyBuilder.ToString();
			return key;
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
			HttpRequestMessage request = actionContext.Request;
			if (request.Method == HttpMethod.Get)
			{
				string useCacheString =
					request.GetQueryNameValuePairs()
						.Where(x => string.Equals(x.Key, "cache", StringComparison.OrdinalIgnoreCase))
						.Select(x => x.Value)
						.FirstOrDefault();
				bool useCache;
				if (!string.IsNullOrEmpty(useCacheString))
				{
					bool.TryParse(useCacheString, out useCache);
				}
				else
				{
					useCache = true;
				}

				if (useCache)
				{
					CacheAttribute cacheAttribute = actionContext.ActionDescriptor.GetCustomAttributes<CacheAttribute>().FirstOrDefault();
					if (cacheAttribute != null)
					{
						HttpResponseMessage httpResponseMessage =
							await ExecuteActionFilterAsyncCore(actionContext, cancellationToken, continuation, cacheAttribute, request);
						return httpResponseMessage;
					}
				}
			}

			return await continuation();
		}

		private async Task<HttpResponseMessage> ExecuteActionFilterAsyncCore(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation, CacheAttribute cacheAttribute, HttpRequestMessage request)
		{
			HttpResponseMessage httpResponseMessage;
			if (cacheAttribute.ServerLifetime > 0)
			{
				string controllerActionName = actionContext.ControllerContext.ControllerDescriptor.ControllerName + "_" + actionContext.ActionDescriptor.ActionName;
				string cacheManagerKey = cacheAttribute.CacheManagerKey ?? controllerActionName;
				ICacheManager cacheManager = _interlockedHelper.CompareExchange(ref _cacheMangers, x =>
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

				string cacheKey = GetKey(request, cacheAttribute.Headers);
				var httpResponseMessageData = (HttpResponseMessageData)cacheManager.GetData(cacheKey);
				string cacheId = cacheAttribute.CacheId ?? controllerActionName;
				DateTime? updatedAt = _webCacheController.GetCacheUpdatedAt(cacheId);
				if (httpResponseMessageData == null || (updatedAt.HasValue && httpResponseMessageData.CreatedAt < updatedAt.Value))
				{
					ILockContainer lockContainer;
					using (_lockStorage.GetContainer(_reflectionHelper.GetFullName(MethodBase.GetCurrentMethod()), out lockContainer))
					{
						ILockAsync @lock;
						using (lockContainer.GetLock(cacheManager, out @lock))
						{
							using (await @lock.Enter(cancellationToken))
							{
								httpResponseMessageData = (HttpResponseMessageData)cacheManager.GetData(cacheKey);
								updatedAt = _webCacheController.GetCacheUpdatedAt(cacheId);
								if (httpResponseMessageData == null || (updatedAt.HasValue && httpResponseMessageData.CreatedAt < updatedAt.Value))
								{
									httpResponseMessage = await continuation();
									if (cacheAttribute.ClientLifetime > 0)
									{
										SetClentCacheControl(httpResponseMessage, cacheAttribute);
									}

									httpResponseMessageData = await HttpResponseMessageData.Create(httpResponseMessage);
									if (httpResponseMessage.IsSuccessStatusCode)
									{
										cacheManager.Add(cacheKey, httpResponseMessageData, CacheItemPriority.Normal, null, new AbsoluteTime(TimeSpan.FromSeconds(cacheAttribute.ServerLifetime)));
									}
								}
							}
						}
					}
				}

				httpResponseMessage = httpResponseMessageData.ToResponseMessage();
			}
			else if (cacheAttribute.ClientLifetime > 0)
			{
				httpResponseMessage = await continuation();
				SetClentCacheControl(httpResponseMessage, cacheAttribute);
			}
			else
			{
				httpResponseMessage = await continuation();
			}

			return httpResponseMessage;
		}

		#endregion
	}
}