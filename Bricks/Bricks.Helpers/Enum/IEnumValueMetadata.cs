#region

using System.Globalization;

#endregion

namespace Bricks.Helpers.Enum
{
	/// <summary>
	/// Интерфейс метаданных значения перечисления.
	/// </summary>
	public interface IEnumValueMetadata
	{
		/// <summary>
		/// Получает название перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Название перечисления.</returns>
		string GetName(CultureInfo cultureInfo = null);

		/// <summary>
		/// Получает описание перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Описание перечисления.</returns>
		string GetDescription(CultureInfo cultureInfo = null);
	}
}