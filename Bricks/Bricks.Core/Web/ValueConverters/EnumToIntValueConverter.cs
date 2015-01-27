namespace Bricks.Core.Web.ValueConverters
{
	public sealed class EnumToIntValueConverter : ValueConverterBase<System.Enum, int>
	{
		#region Overrides of ValueConverterBase<Enum,int>

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected override int Convert(System.Enum source)
		{
			return (int)(object)source;
		}

		#endregion
	}
}