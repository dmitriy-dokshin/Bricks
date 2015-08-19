#region

using System.Globalization;

#endregion

namespace Bricks.Core.Impl.Enumerations
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
		/// Получает метаданные перечисления по ключу.
		/// </summary>
		/// <param name="metadataKey">Ключ метаданных.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Метаданные.</returns>
		string GetEnumMetadata(string metadataKey, CultureInfo cultureInfo);

		/// <summary>
		/// Получает локализованное название значения типа перечисления.
		/// </summary>
		/// <param name="enumValueName">Название значения перечисления.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное название значения типа перечисления.</returns>
		string GetEnumValueName(string enumValueName, CultureInfo cultureInfo);

		/// <summary>
		/// Получает метаданные для знчения перечисления по ключу.
		/// </summary>
		/// <param name="enumValueName">Название значения перечисления.</param>
		/// <param name="metadataKey">Ключ метаданных.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Метаданные.</returns>
		string GetEnumValueMetadata(string enumValueName, string metadataKey, CultureInfo cultureInfo);
	}
}