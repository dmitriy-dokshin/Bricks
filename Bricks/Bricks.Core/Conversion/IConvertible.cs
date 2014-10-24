namespace Bricks.Core.Conversion
{
	/// <summary>
	/// Интерфейс типа, который можно сконвертировать в тип <typeparamref name="TTarget" />.
	/// </summary>
	/// <typeparam name="TTarget">Целевой тип.</typeparam>
	public interface IConvertible<out TTarget>
	{
		/// <summary>
		/// Возвращает экземпляр типа <typeparamref name="TTarget" />, построенный на основе данного.
		/// </summary>
		/// <returns>Экземпляр типа <typeparamref name="TTarget" />.</returns>
		TTarget Convert();
	}
}