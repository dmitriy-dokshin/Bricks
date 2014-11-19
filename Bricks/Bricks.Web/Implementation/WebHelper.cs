#region

using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;

using Bricks.Core.Serialization;

#endregion

namespace Bricks.Web.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IWebHelper" />.
	/// </summary>
	internal sealed class WebHelper : IWebHelper
	{
		private readonly ISerializationHelper _serializationHelper;
		private readonly IWebClient _webClient;
		private readonly IWebSerializationHelper _webSerializationHelper;

		public WebHelper(IWebSerializationHelper webSerializationHelper, ISerializationHelper serializationHelper, IWebClient webClient)
		{
			_webSerializationHelper = webSerializationHelper;
			_serializationHelper = serializationHelper;
			_webClient = webClient;
		}

		#region Implementation of IWebHelper

		/// <summary>
		/// Выполняет "типизированный" запрос по адресу <paramref name="address" /> методом <paramref name="method" />
		/// с параметрами <paramref name="parameters" />.
		/// </summary>
		/// <typeparam name="TParameters">Тип параметров запроса.</typeparam>
		/// <typeparam name="TResult">Тип ответа.</typeparam>
		/// <typeparam name="TErrorResult">Тип ответа в случае ошибки.</typeparam>
		/// <param name="address">Адрес web-сервиса.</param>
		/// <param name="method">Метод запроса.</param>
		/// <param name="parameters">Параметры запроса.</param>
		/// <param name="contentType">Тип контента.</param>
		/// <param name="timeout">Таймаут запроса.</param>
		/// <returns>Кортеж результатов запроса.</returns>
		public async Task<Tuple<TResult, TErrorResult>> Execute<TParameters, TResult, TErrorResult>(Uri address, HttpMethod method, TParameters parameters, ContentType contentType, TimeSpan? timeout = null)
		{
			NameValueCollection data = _webSerializationHelper.ToNameValueCollection(parameters);
			IWebResponse webResponse = await _webClient.ExecuteRequestAsync(address, method, data, timeout: timeout);
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

			return new Tuple<TResult, TErrorResult>(result, errorResult);
		}

		#endregion
	}
}