#region

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

#endregion

namespace Bricks.WebAPI
{
	public sealed class HttpResponseMessageData
	{
		private HttpResponseMessageData(DateTime createdAt, Version version, HttpStatusCode statusCode, string reasonPhrase, HttpResponseHeaders headers)
		{
			CreatedAt = createdAt;
			Version = version;
			StatusCode = statusCode;
			ReasonPhrase = reasonPhrase;
			Headers = headers;
		}

		public DateTime CreatedAt { get; private set; }

		public Version Version { get; private set; }

		public byte[] Content { get; private set; }

		public HttpContentHeaders ContentHeaders { get; private set; }

		public HttpStatusCode StatusCode { get; private set; }

		public string ReasonPhrase { get; private set; }

		public HttpResponseHeaders Headers { get; private set; }

		public static async Task<HttpResponseMessageData> Create(HttpResponseMessage httpResponseMessage)
		{
			var httpResponseMessageData =
				new HttpResponseMessageData(DateTime.UtcNow, httpResponseMessage.Version, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase, httpResponseMessage.Headers);
			if (httpResponseMessage.Content != null)
			{
				httpResponseMessageData.Content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
				httpResponseMessageData.ContentHeaders = httpResponseMessage.Content.Headers;
			}

			return httpResponseMessageData;
		}

		public HttpResponseMessage ToResponseMessage()
		{
			var httpResponseMessage = new HttpResponseMessage(StatusCode) { Version = Version, ReasonPhrase = ReasonPhrase };
			httpResponseMessage.Headers.Clear();
			foreach (KeyValuePair<string, IEnumerable<string>> httpResponseHeader in Headers)
			{
				httpResponseMessage.Headers.Add(httpResponseHeader.Key, httpResponseHeader.Value);
			}

			if (Content != null)
			{
				var content = new ByteArrayContent(Content);
				foreach (KeyValuePair<string, IEnumerable<string>> httpContentHeader in ContentHeaders)
				{
					content.Headers.Add(httpContentHeader.Key, httpContentHeader.Value);
				}

				httpResponseMessage.Content = content;
			}

			return httpResponseMessage;
		}
	}
}