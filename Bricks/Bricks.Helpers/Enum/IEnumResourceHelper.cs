#region

using System.Globalization;

#endregion

namespace Bricks.Helpers.Enum
{
	/// <summary>
	/// Содержит вспомогательные методы для работы с ресурсами перечислений.
	/// </summary>
	internal interface IEnumResourceHelper
	{
		/// <summary>
		/// Получает локализованное название типа перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное название типа перечисления.</returns>
		string GetEnumName(CultureInfo cultureInfo);

		/// <summary>
		/// Получает локализованное описание типа перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное описание типа перечисления.</returns>
		string GetEnumDescription(CultureInfo cultureInfo);

		/// <summary>
		/// Получает локализованное название значения типа перечисления.
		/// </summary>
		/// <param name="enumValueName">Название значения перечисления.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное название значения типа перечисления.</returns>
		string GetEnumValueName(string enumValueName, CultureInfo cultureInfo);

		/// <summary>
		/// Получает локализованное описание значения типа перечисления.
		/// </summary>
		/// <param name="enumValueName">Название значения перечисления.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное описание значения типа перечисления.</returns>
		string GetEnumValueDescription(string enumValueName, CultureInfo cultureInfo);
	}
}