#region

using System;

#endregion

namespace Bricks.Core.Results
{
	/// <summary>
	/// Фабрика создания результатов функций.
	/// </summary>
	public interface IResultFactory
	{
		/// <summary>
		/// Создаёт результат выполнения функции.
		/// </summary>
		/// <param name="success">Признак успешного завершения.</param>
		/// <param name="message">Сообщение, описывающее результат.</param>
		/// <param name="exception">Исключение.</param>
		/// <param name="innerResult">Внутренний результат.</param>
		/// <returns>Результат выполнения функции.</returns>
		IResult Create(bool success = true, string message = null, Exception exception = null, IResult innerResult = null);

		/// <summary>
		/// Создаёт результат выполнения функции с данными.
		/// </summary>
		/// <typeparam name="TData">Тип данных.</typeparam>
		/// <param name="data">Данные.</param>
		/// <param name="success">Признак успешного завершения.</param>
		/// <param name="message">Сообщение, описывающее результат.</param>
		/// <param name="exception">Исключение.</param>
		/// <param name="innerResult">Внутренний результат.</param>
		/// <returns>Результат выполнения функции с данными.</returns>
		IResult<TData> Create<TData>(TData data = default (TData), bool success = true, string message = null, Exception exception = null, IResult innerResult = null);
	}
}