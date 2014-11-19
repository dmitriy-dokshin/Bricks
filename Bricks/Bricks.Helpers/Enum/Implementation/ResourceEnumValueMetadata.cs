﻿#region

using System.Globalization;

#endregion

namespace Bricks.Helpers.Enum.Implementation
{
	/// <summary>
	/// Метаданные значения перечисления, получаемые из ресурсов.
	/// </summary>
	internal class ResourceEnumValueMetadata : IEnumValueMetadata
	{
		private readonly IEnumMetadata _enumMetadata;
		private readonly IEnumResourceHelper _enumResourceHelper;
		private readonly System.Enum _enumValue;

		public ResourceEnumValueMetadata(IEnumMetadata enumMetadata, IEnumResourceHelper enumResourceHelper, System.Enum enumValue)
		{
			_enumMetadata = enumMetadata;
			_enumResourceHelper = enumResourceHelper;
			_enumValue = enumValue;
		}

		#region Implementation of IEnumValueMetadata

		/// <summary>
		/// Получает название перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Название перечисления.</returns>
		public virtual string GetName(CultureInfo cultureInfo = null)
		{
			if (_enumMetadata.ValueNameDictionary.ContainsKey(_enumValue))
			{
				string enumValueName = _enumMetadata.ValueNameDictionary[_enumValue];
				if (cultureInfo != null && cultureInfo.Equals(CultureInfo.InvariantCulture))
				{
					return enumValueName;
				}

				return _enumResourceHelper.GetEnumValueName(enumValueName, cultureInfo);
			}

			return null;
		}

		/// <summary>
		/// Получает описание перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Описание перечисления.</returns>
		public virtual string GetDescription(CultureInfo cultureInfo = null)
		{
			if (_enumMetadata.ValueNameDictionary.ContainsKey(_enumValue))
			{
				string enumValueName = _enumMetadata.ValueNameDictionary[_enumValue];
				return _enumResourceHelper.GetEnumValueDescription(enumValueName, cultureInfo);
			}

			return null;
		}

		#endregion
	}
}