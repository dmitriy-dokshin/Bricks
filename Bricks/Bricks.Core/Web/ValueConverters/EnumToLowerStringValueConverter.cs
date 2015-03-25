#region

using System;

using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Web.ValueConverters
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
		protected override string Convert(Enum source)
		{
			string result = base.Convert(source);
			result = result.ToLowerInvariant();
			return result;
		}

		#endregion
	}
}