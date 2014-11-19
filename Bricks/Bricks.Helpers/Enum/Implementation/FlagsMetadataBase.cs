#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Bricks.Helpers.Enum.Implementation
{
	/// <summary>
	/// Базовый класс метаданных флагового перечисления.
	/// </summary>
	public abstract class FlagsMetadataBase : EnumMetadataBase, IFlagsMetadata
	{
		protected FlagsMetadataBase(Type enumType) : base(enumType, true)
		{
			var flags = new HashSet<System.Enum>(ValueNameDictionary.Keys);
			Flags = flags.Where(x => flags.Count(x.HasFlag) == 1).ToArray();
		}

		#region Implementation of IFlagsMetadata

		/// <summary>
		/// Флаги перечисления.
		/// </summary>
		public IReadOnlyCollection<System.Enum> Flags { get; private set; }

		/// <summary>
		/// Получает метаданные значения флагового перечисления <paramref name="enumValue" />.
		/// </summary>
		/// <param name="enumValue">Значение флагового перечисления.</param>
		/// <returns>Метаданные значения флагового перечисления.</returns>
		public abstract IFlagsValueMetadata GetFlagsValueMetadata(System.Enum enumValue);

		#endregion

		#region Overrides of EnumMetadataBase

		/// <summary>
		/// Получает метаданные значения перечисления <paramref name="enumValue" />.
		/// </summary>
		/// <param name="enumValue">Значение перечисления.</param>
		/// <returns>Метаданные значения перечисления.</returns>
		public override IEnumValueMetadata GetEnumValueMetadata(System.Enum enumValue)
		{
			return GetFlagsValueMetadata(enumValue);
		}

		#endregion
	}
}