#region

using System;
using System.Threading;

#endregion

namespace Bricks.Sync.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IInterlockedHelper" />.
	/// </summary>
	internal sealed class InterlockedHelper : IInterlockedHelper
	{
		#region Implementation of IInterlockedHelper

		/// <summary>
		/// Выполняет неблокирующую замену значения <paramref name="target" />, используя для изменения функцию
		/// <paramref name="change" />.
		/// </summary>
		/// <typeparam name="T">Тип целевого значения.</typeparam>
		/// <param name="target">Значение, которое необходимо заменить.</param>
		/// <param name="change">Функция изменения значения.</param>
		public void CompareExchange<T>(ref T target, Func<T, T> change) where T : class
		{
			T currentValue = Volatile.Read(ref target);
			T oldValue;
			do
			{
				oldValue = currentValue;
				T newValue = change(currentValue);
				currentValue = Interlocked.CompareExchange(ref target, newValue, currentValue);
			} while (!Equals(oldValue, currentValue));
		}

		/// <summary>
		/// Выполняет неблокирующую замену значения <paramref name="target" />, используя для изменения функцию
		/// <paramref name="change" />. Также возвращает предоставляемый функцией <paramref name="change" /> результат.
		/// </summary>
		/// <typeparam name="T">Тип целевого значения.</typeparam>
		/// <typeparam name="TResult">Тип результата.</typeparam>
		/// <param name="target">Значение, которое необходимо заменить.</param>
		/// <param name="change">Функция изменения значения</param>
		/// <returns>Результат замены значения.</returns>
		public TResult CompareExchange<T, TResult>(ref T target, Func<T, IChangeResult<T, TResult>> change) where T : class
		{
			T currentValue = Volatile.Read(ref target);
			T oldValue;
			TResult result;
			do
			{
				oldValue = currentValue;
				IChangeResult<T, TResult> changeResult = change(currentValue);
				currentValue = Interlocked.CompareExchange(ref target, changeResult.NewValue, currentValue);
				result = changeResult.Result;
			} while (!Equals(oldValue, currentValue));

			return result;
		}

		/// <summary>
		/// Создаёт результат изменения значения.
		/// </summary>
		/// <typeparam name="T">Тип целевого значения.</typeparam>
		/// <typeparam name="TResult">Тип результата.</typeparam>
		/// <param name="newValue">Новое целевое значение.</param>
		/// <param name="result">Результат функции изменения.</param>
		/// <returns></returns>
		public IChangeResult<T, TResult> CreateChangeResult<T, TResult>(T newValue, TResult result)
		{
			return new ChangeResult<T, TResult>(newValue, result);
		}

		#endregion

		private sealed class ChangeResult<T, TResult> : IChangeResult<T, TResult>
		{
			public ChangeResult(T newValue, TResult result)
			{
				NewValue = newValue;
				Result = result;
			}

			#region Implementation of IChangeResult<out T,out TResult>

			/// <summary>
			/// Новое целевое значение.
			/// </summary>
			public T NewValue { get; private set; }

			/// <summary>
			/// Результат функции изменения.
			/// </summary>
			public TResult Result { get; private set; }

			#endregion
		}
	}
}