#region

#endregion

namespace Bricks.Core.Regex
{
	/// <summary>
	/// Содержит вспомогательные методы разбора номера мобильного телефона.
	/// </summary>
	public interface IRegexHelper
	{
		/// <summary>
		/// Проверяет корректность email адреса <see cref="source" />.
		/// </summary>
		/// <param name="source">Строка с email адресом.</param>
		/// <returns>Признак корректного email'а.</returns>
		bool IsValidEmail(string source);

		/// <summary>
		/// Пытается разобрать email.
		/// </summary>
		/// <param name="source">Строка с email адресом.</param>
		/// <param name="email">Результат разбора email'а.</param>
		/// <returns>Признак успешного разбора email'а.</returns>
		bool TryParseEmail(string source, out string email);

		/// <summary>
		/// Проверяет корректность номера телефона <paramref name="source" />.
		/// </summary>
		/// <param name="source">Сткрока с номером телефона.</param>
		/// <returns>Признак корректного номера телефона.</returns>
		bool IsValidPhoneNumber(string source);

		/// <summary>
		/// Пытается разобрать номер телефона из строки <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный текст с номером телефона.</param>
		/// <param name="phoneNumber">Результат разбора номера телефона.</param>
		/// <returns>Признак успешного разбора номера телефона.</returns>
		bool TryParsePhoneNumber(string source, out string phoneNumber);

		/// <summary>
		/// Пытается разобрать номер телефона из строки <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный текст с номером телефона.</param>
		/// <param name="country">Код страны (без "+").</param>
		/// <param name="area">Код города, региона или оператора.</param>
		/// <param name="number">Номер телефона.</param>
		/// <returns>Признак успешного разбора номера телефона.</returns>
		bool TryParsePhoneNumber(string source, out string country, out string area, out string number);
	}
}