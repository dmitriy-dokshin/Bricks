#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	internal sealed class EnumMetadataTAdapter<TEnum> : IEnumMetadata<TEnum>
		where TEnum : struct
	{
		private readonly IEnumMetadata _enumMetadata;
		private readonly IReadOnlyDictionary<TEnum, string> _valueNameDictionary;

		public EnumMetadataTAdapter(IEnumMetadata enumMetadata)
		{
			_enumMetadata = enumMetadata;
			_valueNameDictionary = enumMetadata.ValueNameDictionary.ToDictionary(x => (TEnum)(object)x.Key, x => x.Value);
		}

		#region Implementation of IEnumMetadata

		public bool IsFlags
		{
			get
			{
				return _enumMetadata.IsFlags;
			}
		}

		IReadOnlyDictionary<TEnum, string> IEnumMetadata<TEnum>.ValueNameDictionary
		{
			get
			{
				return _valueNameDictionary;
			}
		}

		public IEnumValueMetadata GetEnumValueMetadata(TEnum enumValue)
		{
			return GetEnumValueMetadata((Enum)(object)enumValue);
		}

		IReadOnlyDictionary<Enum, string> IEnumMetadata.ValueNameDictionary
		{
			get
			{
				return _enumMetadata.ValueNameDictionary;
			}
		}

		public IEnumValueMetadata GetEnumValueMetadata(Enum enumValue)
		{
			return _enumMetadata.GetEnumValueMetadata(enumValue);
		}

		public string GetName(CultureInfo cultureInfo = null)
		{
			return _enumMetadata.GetName(cultureInfo);
		}

		public string GetDescription(CultureInfo cultureInfo = null)
		{
			return _enumMetadata.GetDescription(cultureInfo);
		}

		#endregion
	}
}