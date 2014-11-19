namespace Bricks.Web
{
	/// <summary>
	/// Базовый класс для типизированной реализации <see cref="IValueConverter" />.
	/// </summary>
	/// <typeparam name="TSource">Тип исходного значения.</typeparam>
	/// <typeparam name="TTarget">Тип целевого значения.</typeparam>
	public abstract class ValueConverterBase<TSource, TTarget> : IValueConverter
	{
		#region Implementation of IValueConverter

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		public object Convert(object source)
		{
			return Convert((TSource) source);
		}

		#endregion

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected abstract TTarget Convert(TSource source);
	}
}