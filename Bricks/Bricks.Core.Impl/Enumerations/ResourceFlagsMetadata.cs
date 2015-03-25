#region

using System;
using System.Globalization;

using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	/// <summary>
	/// Класс метаданных флагового перечисления, получаемых из ресурсов.
	/// </summary>
	internal sealed class ResourceFlagsMetadata : FlagsMetadataBase
	{
		private readonly IEnumResourceHelper _enumResourceHelper;

		public ResourceFlagsMetadata(Type enumType, IEnumResourceHelper enumResourceHelper)
			: base(enumType)
		{
			_enumResourceHelper = enumResourceHelper;
		}

		#region Overrides of FlagsMetadataBase

		/// <summary>
		/// Получает метаданные значения флагового перечисления <paramref name="enumValue" />.
		/// </summary>
		/// <param name="enumValue">Значение флагового перечисления.</param>
		/// <returns>Метаданные значения флагового перечисления.</returns>
		public override IFlagsValueMetadata GetFlagsValueMetadata(Enum enumValue)
		{
			return new ResourceFlagsValueMetadata(this, _enumResourceHelper, enumValue);
		}

		#endregion

		#region Overrides of EnumMetadataBase

		/// <summary>
		/// Получает название перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Название перечисления.</returns>
		public override string GetName(CultureInfo cultureInfo = null)
		{
			return _enumResourceHelper.GetEnumName(cultureInfo);
		}

		/// <summary>
		/// Получает описание перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Описание перечисления.</returns>
		public override string GetDescription(CultureInfo cultureInfo = null)
		{
			return _enumResourceHelper.GetEnumDescription(cultureInfo);
		}

		#endregion
	}
}