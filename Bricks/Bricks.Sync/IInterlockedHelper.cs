#region

using System;
using System.Threading;

#endregion

namespace Bricks.Sync
{
	/// <summary>
	/// Содержит вспомогательные методы, дополняющие методы класса <see cref="Interlocked" />.
	/// </summary>
	public interface IInterlockedHelper
	{
		/// <summary>
		/// Выполняет неблокирующую замену значения <paramref name="target" />, используя для изменения функцию
		/// <paramref name="change" />.
		/// </summary>
		/// <typeparam name="T">Тип целевого значения.</typeparam>
		/// <param name="target">Значение, которое необходимо заменить.</param>
		/// <param name="change">Функция изменения значения.</param>
		void CompareExchange<T>(ref T target, Func<T, T> change) where T : class;

		/// <summary>
		/// Выполняет неблокирующую замену значения <paramref name="target" />, используя для изменения функцию
		/// <paramref name="change" />. Также возвращает предоставляемый функцией <paramref name="change" /> результат.
		/// </summary>
		/// <typeparam name="T">Тип целевого значения.</typeparam>
		/// <typeparam name="TResult">Тип результата.</typeparam>
		/// <param name="target">Значение, которое необходимо заменить.</param>
		/// <param name="change">Функция изменения значения</param>
		/// <returns>Результат замены значения.</returns>
		TResult CompareExchange<T, TResult>(ref T target, Func<T, IChangeResult<T, TResult>> change) where T : class;

		/// <summary>
		/// Создаёт результат изменения значения.
		/// </summary>
		/// <typeparam name="T">Тип целевого значения.</typeparam>
		/// <typeparam name="TResult">Тип результата.</typeparam>
		/// <param name="newValue">Новое целевое значение.</param>
		/// <param name="result">Результат функции изменения.</param>
		/// <returns></returns>
		IChangeResult<T, TResult> CreateChangeResult<T, TResult>(T newValue, TResult result);
	}
}