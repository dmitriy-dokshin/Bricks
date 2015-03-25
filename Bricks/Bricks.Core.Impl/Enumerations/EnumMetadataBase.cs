#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	/// <summary>
	/// Базовый класс метаданных перечисления.
	/// </summary>
	public abstract class EnumMetadataBase : IEnumMetadata
	{
		protected EnumMetadataBase(Type enumType, bool isFlags = false)
		{
			IsFlags = isFlags;
			ValueNameDictionary = Enum.GetValues(enumType).Cast<Enum>().ToDictionary(x => x, x => Enum.GetName(enumType, x));
		}

		#region Implementation of IEnumMetadata

		/// <summary>
		/// Признак того, что перечисление имеет аттрибут <see cref="FlagsAttribute" />.
		/// </summary>
		public bool IsFlags { get; private set; }

		/// <summary>
		/// Словарь значений-названий перечисления.
		/// </summary>
		public IReadOnlyDictionary<Enum, string> ValueNameDictionary { get; private set; }

		/// <summary>
		/// Получает метаданные значения перечисления <paramref name="enumValue" />.
		/// </summary>
		/// <param name="enumValue">Значение перечисления.</param>
		/// <returns>Метаданные значения перечисления.</returns>
		public abstract IEnumValueMetadata GetEnumValueMetadata(Enum enumValue);

		/// <summary>
		/// Получает название перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Название перечисления.</returns>
		public abstract string GetName(CultureInfo cultureInfo = null);

		/// <summary>
		/// Получает описание перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Описание перечисления.</returns>
		public abstract string GetDescription(CultureInfo cultureInfo = null);

		#endregion
	}
}