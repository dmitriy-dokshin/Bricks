#region

using System;

#endregion

namespace Bricks.Core.Regex
{
	/// <summary>
	/// Настройки регулярных выражений.
	/// </summary>
	public interface IRegexSettings
	{
		/// <summary>
		/// Настройки email'а.
		/// </summary>
		IEmailRegexSettings Email { get; }

		/// <summary>
		/// Настройки номера телефона.
		/// </summary>
		IPhoneRegexSettings Phone { get; }

		/// <summary>
		/// Таймаут разбора в милисекундах.
		/// </summary>
		TimeSpan MatchTimeout { get; }
	}
}