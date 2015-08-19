#region

using System.Globalization;

#endregion

namespace Bricks.Core.Enumerations
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
		/// Получает метаданные для знчения перечисления по ключу.
		/// </summary>
		/// <param name="metadataKey">Ключ метаданных.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Метаданные.</returns>
		string GetMetadata(string metadataKey, CultureInfo cultureInfo = null);
	}
}