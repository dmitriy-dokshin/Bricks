#region

using System;

#endregion

namespace Bricks.Helpers.Exceptions.Implementation
{
	/// <summary>
	/// Помощник работы с исключениями.
	/// </summary>
	internal sealed class ExceptionHelper : IExceptionHelper
	{
		#region Implementation of IExceptionHelper

		/// <summary>
		/// Игнорирует исключение типа <see cref="TException" />, если оно возникает при выполнении функции
		/// <paramref name="func" />.
		/// </summary>
		/// <typeparam name="TResult">Тип результата.</typeparam>
		/// <typeparam name="TException">Тип исключения.</typeparam>
		/// <param name="func">Функция, которую необходимо выполнить.</param>
		/// <returns>Результат выполнения функции или значение по умолчанию, если возникло исключение.</returns>
		public TResult Catch<TResult, TException>(Func<TResult> func) where TException : Exception
		{
			try
			{
				return func();
			}
			catch (Exception ex)
			{
				if (ex is TException)
				{
					return default(TResult);
				}

				throw;
			}
		}

		#endregion
	}
}