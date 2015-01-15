#region

using System;
using System.Collections.Generic;
using System.Globalization;

#endregion

namespace Bricks.Core.Enum
{
	/// <summary>
	/// Интерфейс метаданных перечисления.
	/// </summary>
	public interface IEnumMetadata
	{
		/// <summary>
		/// Признак того, что перечисление имеет аттрибут <see cref="FlagsAttribute" />.
		/// </summary>
		bool IsFlags { get; }

		/// <summary>
		/// Словарь значений-названий перечисления.
		/// </summary>
		IReadOnlyDictionary<System.Enum, string> ValueNameDictionary { get; }

		/// <summary>
		/// Получает метаданные значения перечисления <paramref name="enumValue" />.
		/// </summary>
		/// <param name="enumValue">Значение перечисления.</param>
		/// <returns>Метаданные значения перечисления.</returns>
		IEnumValueMetadata GetEnumValueMetadata(System.Enum enumValue);

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

	public interface IEnumMetadata<TEnum> : IEnumMetadata
		where TEnum : struct
	{
		/// <summary>
		/// Словарь значений-названий перечисления.
		/// </summary>
		new IReadOnlyDictionary<TEnum, string> ValueNameDictionary { get; }

		IEnumValueMetadata GetEnumValueMetadata(TEnum enumValue);
	}
}