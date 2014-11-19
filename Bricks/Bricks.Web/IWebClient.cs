#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace Bricks.Web
{
	/// <summary>
	/// Интерфейс, объединяющий методы работы с web-сервисами.
	/// </summary>
	public interface IWebClient
	{
		/// <summary>
		/// Выполняет запрос по адресу <paramref name="address" /> методом <paramref name="method" />.
		/// </summary>
		/// <param name="address">Адрес web-сервиса.</param>
		/// <param name="method">Метод запроса.</param>
		/// <param name="data">Параметры запроса.</param>
		/// <param name="headers">Заголовки.</param>
		/// <param name="timeout">Таймаут запроса.</param>
		/// <returns>Ответ на запрос.</returns>
		Task<IWebResponse> ExecuteRequestAsync(Uri address, HttpMethod method, NameValueCollection data, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null, TimeSpan? timeout = null);
	}
}