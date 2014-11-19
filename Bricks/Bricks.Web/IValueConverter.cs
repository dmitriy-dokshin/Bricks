#region

#endregion

namespace Bricks.Web
{
	/// <summary>
	/// Интерфейс конвертера значений.
	/// </summary>
	public interface IValueConverter
	{
		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		object Convert(object source);
	}
}