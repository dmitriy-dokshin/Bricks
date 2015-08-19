#region

using System;
using System.Globalization;
using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	/// <summary>
	/// Метаданные значения перечисления, получаемые из ресурсов.
	/// </summary>
	internal class ResourceEnumValueMetadata : IEnumValueMetadata
	{
		private readonly IEnumMetadata _enumMetadata;
		private readonly IEnumResourceHelper _enumResourceHelper;
		private readonly Enum _enumValue;

		public ResourceEnumValueMetadata(IEnumMetadata enumMetadata, IEnumResourceHelper enumResourceHelper, Enum enumValue)
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
		/// Получает метаданные для знчения перечисления по ключу.
		/// </summary>
		/// <param name="metadataKey">Ключ метаданных.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Метаданные.</returns>
		public virtual string GetMetadata(string metadataKey, CultureInfo cultureInfo = null)
		{
			if (_enumMetadata.ValueNameDictionary.ContainsKey(_enumValue))
			{
				string enumValueName = _enumMetadata.ValueNameDictionary[_enumValue];
				return _enumResourceHelper.GetEnumValueMetadata(enumValueName, metadataKey, cultureInfo);
			}

			return null;
		}

		#endregion
	}
}