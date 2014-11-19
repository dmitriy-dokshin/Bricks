#region

using System;
using System.Collections.Generic;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Настройки Nexmo.
	/// </summary>
	public interface INexmoSettings
	{
		/// <summary>
		/// Ключ приложения.
		/// </summary>
		string Key { get; }

		/// <summary>
		/// Секретный ключ.
		/// </summary>
		string Secret { get; }

		/// <summary>
		/// Базовый URL.
		/// </summary>
		Uri BaseUrl { get; }

		/// <summary>
		/// Адреса сервисов.
		/// </summary>
		IReadOnlyDictionary<Type, Uri> ServiceUrls { get; }

		/// <summary>
		/// Идентификатор отправителя.
		/// </summary>
		string SenderId { get; }
	}
}