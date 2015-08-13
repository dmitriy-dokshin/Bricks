#region

using System;
using Bricks.Core.DateTime;

#endregion

namespace Bricks.Core.Web.ValueConverters
{
	public sealed class DateTimeToUnixTimeValueConverter : ValueConverterBase<DateTimeOffset, double>
	{
		#region Overrides of ValueConverterBase<DateTimeOffset,double>

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected override double Convert(DateTimeOffset source)
		{
			return DateTimeHelper.ToUnixTime(source);
		}

		#endregion
	}
}