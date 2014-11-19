#region

using System;
using System.Collections.Specialized;

#endregion

namespace Bricks.Web.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IWebHelper" />.
	/// </summary>
	internal sealed class UrlHelper : IUrlHelper
	{
		private readonly IWebSerializationHelper _webSerializationHelper;

		public UrlHelper(IWebSerializationHelper webSerializationHelper)
		{
			_webSerializationHelper = webSerializationHelper;
		}

		#region Implementation of IUrlHelper

		/// <summary>
		/// Получает URL <paramref name="address" /> с параметрами <paramref name="parameters" />.
		/// </summary>
		/// <typeparam name="TParameters">Тип параметров.</typeparam>
		/// <param name="address">Адрес web-сервиса.</param>
		/// <param name="parameters">Параметры.</param>
		/// <returns>URL с параметрами.</returns>
		public Uri GetUrl<TParameters>(Uri address, TParameters parameters)
		{
			var urlBuilder = new UriBuilder(address);
			NameValueCollection nameValueCollection = _webSerializationHelper.ToNameValueCollection(parameters);
			urlBuilder.AppendQuery(nameValueCollection);
			return urlBuilder.Uri;
		}

		#endregion
	}
}