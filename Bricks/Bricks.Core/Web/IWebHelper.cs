#region

using System;
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
		/// <param name="method">Метод запроса.</param>
		/// <param name="parameters">Параметры запроса.</param>
		/// <param name="contentType">Тип контента.</param>
		/// <param name="timeout">Таймаут запроса.</param>
		/// <returns>Результат запроса.</returns>
		Task<IResult<WebResponseData<TResult, TErrorResult>>> Execute<TParameters, TResult, TErrorResult>(
			Uri address, HttpMethod method, TParameters parameters, ContentType contentType, TimeSpan? timeout = null);
	}
}