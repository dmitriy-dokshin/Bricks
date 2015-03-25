#region

using System;
using System.Collections.Generic;

#endregion

namespace Bricks.Core.Enumerations
{
	/// <summary>
	/// Интерфейс метаданных флагового перечисления.
	/// </summary>
	public interface IFlagsMetadata : IEnumMetadata
	{
		/// <summary>
		/// Флаги перечисления.
		/// </summary>
		IReadOnlyCollection<Enum> Flags { get; }

		/// <summary>
		/// Получает метаданные значения флагового перечисления <paramref name="enumValue" />.
		/// </summary>
		/// <param name="enumValue">Значение флагового перечисления.</param>
		/// <returns>Метаданные значения флагового перечисления.</returns>
		IFlagsValueMetadata GetFlagsValueMetadata(Enum enumValue);
	}
}