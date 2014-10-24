namespace Bricks.Core.Conversion
{
	/// <summary>
	/// Содержит методы приведения типов.
	/// </summary>
	public interface IConverter
	{
		/// <summary>
		/// Пытается привести объект <paramref name="source" /> к типу <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <param name="destination">Объект целевого типа или значение по умолчанию, если не удалось сконвертировать.</param>
		/// <returns>Признак успешной конвертации.</returns>
		bool TryConvert<TDestination>(object source, out TDestination destination);

		/// <summary>
		/// Приводит объект <paramref name="source" /> к типу <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Объект целевого типа.</returns>
		/// <exception cref="ConversionException">В случае если не выполнить конвертацию.</exception>
		TDestination Convert<TDestination>(object source);

		/// <summary>
		/// Регистрирует конвертер типа <typeparamref name="TConverter" /> для приведения к типу
		/// <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <typeparam name="TConverter">Тип конвертера.</typeparam>
		void Register<TDestination, TConverter>() where TConverter : IConverter<TDestination>;

		/// <summary>
		/// Регистриует конвертер <paramref name="converter" /> для приведения к типу <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="converter">Конвертер.</param>
		void Register<TDestination>(IConverter<TDestination> converter);
	}

	/// <summary>
	/// Конфертер объектов к типу <typeparamref name="TDestination" />.
	/// </summary>
	/// <typeparam name="TDestination">Целевой тип.</typeparam>
	public interface IConverter<TDestination>
	{
		/// <summary>
		/// Пытается привести объект <paramref name="source" /> к типу <typeparamref name="TDestination" />.
		/// </summary>
		/// <param name="source">Исходный объект.</param>
		/// <param name="destination">Объект целевого типа или значение по умолчанию, если не удалось сконвертировать.</param>
		/// <returns>Признак успешной конвертации.</returns>
		bool TryConvert(object source, out TDestination destination);
	}
}