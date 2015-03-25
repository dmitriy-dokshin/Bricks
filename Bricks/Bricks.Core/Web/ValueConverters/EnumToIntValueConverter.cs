#region

using System;

#endregion

namespace Bricks.Core.Web.ValueConverters
{
	public sealed class EnumToIntValueConverter : ValueConverterBase<Enum, int>
	{
		#region Overrides of ValueConverterBase<Enum,int>

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected override int Convert(Enum source)
		{
			return (int)(object)source;
		}

		#endregion
	}
}