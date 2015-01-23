#region

using System;
using System.IO;
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

		public async Task<IResult<WebResponseData<TResult, TErrorResult>>> Execute<TParameters, TResult, TErrorResult>(Uri address, HttpMethod method, TParameters parameters, ContentType contentType, TimeSpan? timeout = null)
		{
			var data = _webSerializationHelper.ToNameValueCollection(parameters);
			var webResponse = await _webClient.ExecuteRequestAsync(address, method, data, timeout: timeout);
			TResult result;
			TErrorResult errorResult;
			if (webResponse.Success)
			{
				switch (contentType)
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
						result = _serializationHelper.DeserializeJson<TResult>(webResponse.Stream);
						break;
					default:
						throw new ArgumentOutOfRangeException("contentType");
				}

				errorResult = default (TErrorResult);
			}
			else
			{
				switch (contentType)
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
						errorResult = _serializationHelper.DeserializeJson<TErrorResult>(webResponse.Stream);
						break;
					default:
						throw new ArgumentOutOfRangeException("contentType");
				}

				result = default (TResult);
			}

			return _resultFactory.Create(new WebResponseData<TResult, TErrorResult>(result, errorResult), webResponse.Success, exception: webResponse.Exception);
		}

		#endregion
	}
}