#region

using System;
using System.IO;

using Bricks.Core.Web;

#endregion

namespace Bricks.Core.Impl.Web
{
	/// <summary>
	/// Реализацию по умолчанию <see cref="IWebResponse" />.
	/// </summary>
	internal sealed class WebResponse : IWebResponse
	{
		public WebResponse(bool success, Stream stream, Exception exception)
		{
			Success = success;
			Stream = stream;
			Exception = exception;
		}

		#region Implementation of IWebResponse

		/// <summary>
		/// Признак успешного запроса.
		/// </summary>
		public bool Success { get; private set; }

		/// <summary>
		/// Поток данных ответа.
		/// </summary>
		public Stream Stream { get; private set; }

		/// <summary>
		/// Информация об исключении, возникшем в ходе запроса.
		/// </summary>
		public Exception Exception { get; private set; }

		#endregion
	}
}