#region

using System;

#endregion

namespace Bricks.Web.ValueConverters
{
	/// <summary>
	/// Конвертер значений <see cref="Guid" /> в строки.
	/// </summary>
	public sealed class GuidToStringValueConverter : ValueConverterBase<Guid, string>
	{
		#region Overrides of ValueConverterBase<Guid,string>

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected override string Convert(Guid source)
		{
			return source.ToString("D");
		}

		#endregion
	}
}