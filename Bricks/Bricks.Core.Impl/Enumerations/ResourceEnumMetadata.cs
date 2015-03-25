#region

using System;
using System.Globalization;

using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	/// <summary>
	/// Класс метаданных перечисления, получаемых из ресурсов.
	/// </summary>
	internal class ResourceEnumMetadata : EnumMetadataBase
	{
		private readonly IEnumResourceHelper _enumResourceHelper;

		public ResourceEnumMetadata(Type enumType, IEnumResourceHelper enumResourceHelper)
			: base(enumType)
		{
			_enumResourceHelper = enumResourceHelper;
		}

		#region Overrides of EnumMetadataBase

		/// <summary>
		/// Получает метаданные значения перечисления <paramref name="enumValue" />.
		/// </summary>
		/// <param name="enumValue">Значение перечисления.</param>
		/// <returns>Метаданные значения перечисления.</returns>
		public override IEnumValueMetadata GetEnumValueMetadata(Enum enumValue)
		{
			return new ResourceEnumValueMetadata(this, _enumResourceHelper, enumValue);
		}

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