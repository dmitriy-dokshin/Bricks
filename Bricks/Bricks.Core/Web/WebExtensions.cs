#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

#endregion

namespace Bricks.Core.Web
{
	public static class WebExtensions
	{
		private const string QueryParamsSeparator = "&";
		private const string QueryParamFormat = "{0}={1}";

		public static void AppendQuery(this UriBuilder uriBuilder, NameValueCollection data)
		{
			uriBuilder.AppendQuery(data.GetEnumerable());
		}

		public static void AppendQuery(this UriBuilder uriBuilder, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			string queryToAppend =
				string.Join(QueryParamsSeparator, parameters.Select(x => string.Format(CultureInfo.InvariantCulture, QueryParamFormat, x.Key, Uri.EscapeDataString(x.Value))));
			if (uriBuilder.Query.Length > 1)
			{
				uriBuilder.Query = uriBuilder.Query.Substring(1) + QueryParamsSeparator + queryToAppend;
			}
			else
			{
				uriBuilder.Query = queryToAppend;
			}
		}

		/// <summary>
		/// Получает перечисление для коллекции <paramref name="nameValueCollection" />.
		/// </summary>
		/// <param name="nameValueCollection">Коллекция значений.</param>
		/// <returns>Перечисление для коллекции <paramref name="nameValueCollection" />.</returns>
		public static IEnumerable<KeyValuePair<string, string>> GetEnumerable(this NameValueCollection nameValueCollection)
		{
			return from string key in nameValueCollection.Keys
				   let values = nameValueCollection.GetValues(key)
				   where values != null
				   from value in values
				   select new KeyValuePair<string, string>(key, value);
		}

		/// <summary>
		/// Создаёт абсолютный URL на основе базового <paramref name="baseUrl" />,
		/// если указанный URL <paramref name="target" /> является относительным.
		/// </summary>
		/// <param name="target">Целевой URL.</param>
		/// <param name="baseUrl">Базовый URL.</param>
		/// <returns>Абсолютный URL.</returns>
		public static Uri ToAbsoluteIfNot(this Uri target, Uri baseUrl)
		{
			return target.IsAbsoluteUri ? target : new Uri(baseUrl, target);
		}
	}
}