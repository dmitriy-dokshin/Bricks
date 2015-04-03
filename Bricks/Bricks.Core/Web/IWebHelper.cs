#region

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Web
{
	/// <summary>
	/// Содержит вспомогательные методы для работы с web-сервисами.
	/// </summary>
	public interface IWebHelper
	{
		/// <summary>
		/// Выполняет "типизированный" запрос по адресу <paramref name="address" /> методом <paramref name="method" />
		/// с параметрами <paramref name="parameters" />.
		/// </summary>
		/// <typeparam name="TParameters">Тип параметров запроса.</typeparam>
		/// <typeparam name="TResult">Тип ответа.</typeparam>
		/// <typeparam name="TErrorResult">Тип ответа в случае ошибки.</typeparam>
		/// <param name="address">Адрес web-сервиса.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <param name="parameters">Параметры запроса.</param>
		/// <param name="method">Метод запроса.</param>
		/// <param name="resultContentType">Тип контента.</param>
		/// <param name="errorContentType">Тип контента ошибки.</param>
		/// <param name="headers">Заголовки.</param>
		/// <param name="timeout">Таймаут запроса.</param>
		/// <returns>Результат запроса.</returns>
		Task<IResult<WebResponseData<TResult, TErrorResult>>> Execute<TParameters, TResult, TErrorResult>(
			Uri address, CancellationToken cancellationToken, TParameters parameters, HttpMethod method = HttpMethod.Get, ContentType? resultContentType = null, ContentType? errorContentType = null,
			IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null, TimeSpan? timeout = null);

		Task<IResult<WebResponseData<TResult, TErrorResult>>> Execute<TResult, TErrorResult>(
			Uri address, CancellationToken cancellationToken, HttpMethod method = HttpMethod.Get, ContentType? resultContentType = null, ContentType? errorContentType = null,
			IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null, TimeSpan? timeout = null);
	}
}