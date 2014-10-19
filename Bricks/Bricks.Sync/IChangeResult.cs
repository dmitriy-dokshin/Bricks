namespace Bricks.Sync
{
	/// <summary>
	/// Представляет результат функции изменения для метода <see cref="IInterlockedHelper.CompareExchange{T,TResult}" />.
	/// </summary>
	/// <typeparam name="T">Тип целевого значения.</typeparam>
	/// <typeparam name="TResult">Тип результата.</typeparam>
	public interface IChangeResult<out T, out TResult>
	{
		/// <summary>
		/// Новое целевое значение.
		/// </summary>
		T NewValue { get; }

		/// <summary>
		/// Результат функции изменения.
		/// </summary>
		TResult Result { get; }
	}
}