#region

using System;
using System.IO;

#endregion

namespace Bricks.Core.Web
{
	/// <summary>
	/// Интерфейс ответа на web-запрос.
	/// </summary>
	public interface IWebResponse
	{
		/// <summary>
		/// Признак успешного запроса.
		/// </summary>
		bool Success { get; }

		/// <summary>
		/// Поток данных ответа.
		/// </summary>
		Stream Stream { get; }

		/// <summary>
		/// Информация об исключении, возникшем в ходе запроса.
		/// </summary>
		Exception Exception { get; }
	}
}