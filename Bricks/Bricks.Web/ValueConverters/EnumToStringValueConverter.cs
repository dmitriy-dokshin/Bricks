#region

using System;
using System.Globalization;

using Bricks.Helpers.Enum;

#endregion

namespace Bricks.Web.ValueConverters
{
	/// <summary>
	/// Конвертер значений перечислений в строки.
	/// </summary>
	public class EnumToStringValueConverter : ValueConverterBase<Enum, string>
	{
		private readonly IEnumMetadataContainer _enumMetadataContainer;

		protected EnumToStringValueConverter(IEnumMetadataContainer enumMetadataContainer)
		{
			_enumMetadataContainer = enumMetadataContainer;
		}

		#region Overrides of ValueConverterBase<Enum,string>

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected override string Convert(Enum source)
		{
			IEnumMetadata enumMetadata = _enumMetadataContainer.GetEnumMetadata(source.GetType());
			IEnumValueMetadata enumValueMetadata = enumMetadata.GetEnumValueMetadata(source);
			return enumValueMetadata.GetName(CultureInfo.InvariantCulture);
		}

		#endregion
	}
}