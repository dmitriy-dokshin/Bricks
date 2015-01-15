#region

using Bricks.Core.Enum;

#endregion

namespace Bricks.Core.Impl.Web.ValueConverters
{
	/// <summary>
	/// Конвертер значений перечислений в строки нижнего регистра.
	/// </summary>
	public sealed class EnumToLowerStringValueConverter : EnumToStringValueConverter
	{
		public EnumToLowerStringValueConverter(IEnumMetadataContainer enumMetadataContainer)
			: base(enumMetadataContainer)
		{
		}

		#region Overrides of EnumToStringValueConverter

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected override string Convert(System.Enum source)
		{
			var result = base.Convert(source);
			result = result.ToLowerInvariant();
			return result;
		}

		#endregion
	}
}