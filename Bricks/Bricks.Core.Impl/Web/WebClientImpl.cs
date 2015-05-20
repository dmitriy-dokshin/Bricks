#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Configuration;
using Bricks.Core.Tasks;
using Bricks.Core.Web;

#endregion

namespace Bricks.Core.Impl.Web
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IWebClient" />.
	/// </summary>
	internal sealed class WebClientImpl : IWebClient
	{
		private readonly IPAddress _ipAddress;

		public WebClientImpl(IConfigurationManager configurationManager)
		{
			string ipString = configurationManager.AppSettings["WebClient_IPAddress"];
			if (!string.IsNullOrEmpty(ipString))
			{
				_ipAddress = IPAddress.Parse(ipString);
			}
		}

		#region Implementation of IWebClient

		/// <summary>
		/// Выполняет запрос по адресу <paramref name="address" /> методом <paramref name="method" />.
		/// </summary>
		/// <param name="address">Адрес web-сервиса.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <param name="data">Параметры запроса.</param>
		/// <param name="method">Метод запроса.</param>
		/// <param name="headers">Заголовки.</param>
		/// <param name="timeout">Таймаут запроса.</param>
		/// <returns>Ответ на запрос.</returns>
		public async Task<IWebResponse> ExecuteRequestAsync(Uri address, CancellationToken cancellationToken, NameValueCollection data = null, HttpMethod method = HttpMethod.Get, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers = null, TimeSpan? timeout = null)
		{
			if (!timeout.HasValue)
			{
				timeout = TimeSpan.FromSeconds(30);
			}

			bool success;
			Stream stream = null;
			Exception exception = null;
			using (var webClient = new WebClientWithTimeout(timeout.Value, cancellationToken, _ipAddress))
			{
				if (headers != null)
				{
					foreach (KeyValuePair<HttpRequestHeader, string> header in headers)
					{
						webClient.Headers.Add(header.Key, header.Value);
					}
				}

				try
				{
					byte[] bytes;
					if (method == HttpMethod.Get)
					{
						var addressBuilder = new UriBuilder(address);
						if (data != null)
						{
							addressBuilder.AppendQuery(data);
						}

						address = addressBuilder.Uri;
						bytes = await webClient.DownloadDataTaskAsync(address).WithTimeOut(timeout.Value);
					}
					else
					{
						if (data == null)
						{
							throw new ArgumentNullException("data");
						}

						bytes = await webClient.UploadValuesTaskAsync(address, method.ToString(), data).WithTimeOut(timeout.Value);
					}

					success = true;
					stream = new MemoryStream(bytes);
				}
				catch (WebException webException)
				{
					success = false;
					if (webException.Response != null)
					{
						stream = webException.Response.GetResponseStream();
					}

					exception = webException;
				}
				catch (TimeoutException timeoutException)
				{
					success = false;
					exception = timeoutException;
				}
			}

			var webResponse = new WebResponse(success, stream, exception);
			return webResponse;
		}

		#endregion

		private sealed class WebClientWithTimeout : WebClient
		{
			private readonly TimeSpan _timeout;
			private readonly IPAddress _ipAddress;
			private CancellationTokenRegistration _cancellationTokenRegistration;

			public WebClientWithTimeout(TimeSpan timeout, CancellationToken cancellationToken, IPAddress ipAddress)
			{
				_timeout = timeout;
				_ipAddress = ipAddress;
				_cancellationTokenRegistration = cancellationToken.Register(CancelAsync);
			}

			#region Overrides of WebClient

			/// <summary>
			/// Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.
			/// </summary>
			/// <returns>
			/// A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.
			/// </returns>
			/// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
			protected override WebRequest GetWebRequest(Uri address)
			{
				WebRequest webRequest = base.GetWebRequest(address);
				if (webRequest != null)
				{
					webRequest.Timeout = Convert.ToInt32(Math.Round(_timeout.TotalMilliseconds));
				}

				if (_ipAddress != null)
				{
					var httpWebRequest = webRequest as HttpWebRequest;
					if (httpWebRequest != null)
					{
						httpWebRequest.ServicePoint.BindIPEndPointDelegate += (point, endPoint, count) => new IPEndPoint(_ipAddress, 0);
					}
				}

				return webRequest;
			}

			#endregion

			#region Overrides of Component

			/// <summary>
			/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"/> and optionally releases the managed resources.
			/// </summary>
			/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if (disposing)
				{
					_cancellationTokenRegistration.Dispose();
				}
			}

			#endregion
		}
	}
}