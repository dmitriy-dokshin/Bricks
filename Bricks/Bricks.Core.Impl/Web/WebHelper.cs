#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Results;
using Bricks.Core.Serialization;
using Bricks.Core.Web;

#endregion

namespace Bricks.Core.Impl.Web
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IWebHelper" />.
	/// </summary>
	internal sealed class WebHelper : IWebHelper
	{
		private readonly IResultFactory _resultFactory;
		private readonly ISerializationHelper _serializationHelper;
		private readonly IWebClient _webClient;
		private readonly IWebSerializationHelper _webSerializationHelper;

		public WebHelper(IWebSerializationHelper webSerializationHelper, ISerializationHelper serializationHelper, IWebClient webClient, IResultFactory resultFactory)
		{
			_webSerializationHelper = webSerializationHelper;
			_serializationHelper = serializationHelper;
			_webClient = webClient;
			_resultFactory = resultFactory;
		}

		#region Implementation of IWebHelper

		public Task<IResult<WebResponseData<TResult, TErrorResult>>> Execute<TParameters, TResult, TErrorResult>(
			Uri address, CancellationToken cancellationToken, TParameters parameters, HttpMethod method = HttpMethod.Get, ContentType? resultContentType = null, ContentType? errorContentType = null,
			IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null, TimeSpan? timeout = null)
		{
			NameValueCollection data = _webSerializationHelper.ToNameValueCollection(parameters);
			return ExecuteCore<TResult, TErrorResult>(address, cancellationToken, method, resultContentType, errorContentType, headers, timeout, data);
		}

		public Task<IResult<WebResponseData<TResult, TErrorResult>>> Execute<TResult, TErrorResult>(
			Uri address, CancellationToken cancellationToken, HttpMethod method = HttpMethod.Get, ContentType? resultContentType = null, ContentType? errorContentType = null,
			IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null, TimeSpan? timeout = null)
		{
			return ExecuteCore<TResult, TErrorResult>(address, cancellationToken, method, resultContentType, errorContentType, headers, timeout);
		}

		private async Task<IResult<WebResponseData<TResult, TErrorResult>>> ExecuteCore<TResult, TErrorResult>(
			Uri address, CancellationToken cancellationToken, HttpMethod method = HttpMethod.Get, ContentType? resultContentType = null, ContentType? errorContentType = null,
			IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null, TimeSpan? timeout = null, NameValueCollection data = null)
		{
			if (!resultContentType.HasValue)
			{
				resultContentType = typeof(TResult) == typeof(string) ? ContentType.String : ContentType.Json;
			}

			if (!errorContentType.HasValue)
			{
				errorContentType = typeof(TErrorResult) == typeof(string) ? ContentType.String : ContentType.Json;
			}

			IWebResponse webResponse = await _webClient.ExecuteRequestAsync(address, cancellationToken, data, method, headers, timeout);
			TResult result;
			TErrorResult errorResult;
			if (webResponse.Stream != null)
			{
				if (webResponse.Success)
				{
					switch (resultContentType.Value)
					{
						case ContentType.String:
							if (typeof(TResult) != typeof(string))
							{
								throw new InvalidCastException();
							}

							var streamReader = new StreamReader(webResponse.Stream);
							result = (TResult)(object)await streamReader.ReadToEndAsync();
							break;
						case ContentType.Json:
							var deserializeJsonResult = _serializationHelper.DeserializeJson<TResult>(webResponse.Stream);
							if (!deserializeJsonResult.Success)
							{
								return _resultFactory.CreateUnsuccessfulResult<WebResponseData<TResult, TErrorResult>>(innerResult: deserializeJsonResult);
							}

							result = deserializeJsonResult.Data;
							break;
						default:
							throw new ArgumentOutOfRangeException("resultContentType");
					}

					errorResult = default(TErrorResult);
				}
				else
				{
					switch (errorContentType.Value)
					{
						case ContentType.String:
							if (typeof(TErrorResult) != typeof(string))
							{
								throw new InvalidCastException();
							}

							var streamReader = new StreamReader(webResponse.Stream);
							errorResult = (TErrorResult)(object)await streamReader.ReadToEndAsync();
							break;
						case ContentType.Json:
							var deserializeJsonResult = _serializationHelper.DeserializeJson<TErrorResult>(webResponse.Stream);
							if (!deserializeJsonResult.Success)
							{
								return _resultFactory.CreateUnsuccessfulResult<WebResponseData<TResult, TErrorResult>>(innerResult: deserializeJsonResult);
							}

							errorResult = deserializeJsonResult.Data;
							break;
						default:
							throw new ArgumentOutOfRangeException("resultContentType");
					}

					result = default(TResult);
				}
			}
			else
			{
				result = default (TResult);
				errorResult = default(TErrorResult);
			}

			return _resultFactory.Create(new WebResponseData<TResult, TErrorResult>(result, errorResult), webResponse.Success, exception: webResponse.Exception);
		}

		#endregion
	}
}