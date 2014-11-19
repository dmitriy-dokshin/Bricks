#region

using System;

#endregion

namespace Bricks.Helpers.Exceptions
{
	public interface IExceptionHelper
	{
		/// <summary>
		/// Игнорирует исключение типа <see cref="TException" />, если оно возникает при выполнении функции
		/// <paramref name="func" />.
		/// </summary>
		/// <typeparam name="TResult">Тип результата.</typeparam>
		/// <typeparam name="TException">Тип исключения.</typeparam>
		/// <param name="func">Функция, которую необходимо выполнить.</param>
		/// <returns>Результат выполнения функции или значение по умолчанию, если возникло исключение.</returns>
		TResult Catch<TResult, TException>(Func<TResult> func)
		where TException : Exception;
	}
}