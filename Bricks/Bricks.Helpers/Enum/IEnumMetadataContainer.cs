#region

using System;

#endregion

namespace Bricks.Helpers.Enum
{
	/// <summary>
	/// Контейнер метаданных перечислений.
	/// </summary>
	public interface IEnumMetadataContainer
	{
		/// <summary>
		/// Получает метаданные перечисления типа <paramref name="enumType" />.
		/// </summary>
		/// <param name="enumType">Тип перечисления.</param>
		/// <returns>Метаданные перечисления.</returns>
		IEnumMetadata GetEnumMetadata(Type enumType);
	}
}