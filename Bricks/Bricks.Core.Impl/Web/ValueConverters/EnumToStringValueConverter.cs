#region

using System.Globalization;

using Bricks.Core.Enum;
using Bricks.Core.Web;

#endregion

namespace Bricks.Core.Impl.Web.ValueConverters
{
	/// <summary>
	/// Конвертер значений перечислений в строки.
	/// </summary>
	public class EnumToStringValueConverter : ValueConverterBase<System.Enum, string>
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
		protected override string Convert(System.Enum source)
		{
			var enumMetadata = _enumMetadataContainer.GetEnumMetadata(source.GetType());
			var enumValueMetadata = enumMetadata.GetEnumValueMetadata(source);
			return enumValueMetadata.GetName(CultureInfo.InvariantCulture);
		}

		#endregion
	}
}