#region

using System;

#endregion

namespace Bricks.Core.Results
{
	/// <summary>
	/// Описывает результат выполнения функции.
	/// </summary>
	public interface IResult
	{
		/// <summary>
		/// Признак успешного завершения.
		/// </summary>
		bool Success { get; }

		/// <summary>
		/// Сообщение, описывающее результат.
		/// </summary>
		string Message { get; }

		/// <summary>
		/// Исключение.
		/// </summary>
		Exception Exception { get; }

		/// <summary>
		/// Внутренний результат.
		/// </summary>
		IResult InnerResult { get; }
	}

	/// <summary>
	/// Описывает результат выполнения функции с данными.
	/// </summary>
	/// <typeparam name="TData">Тип данных.</typeparam>
	public interface IResult<out TData> : IResult
	{
		/// <summary>
		/// Данные.
		/// </summary>
		TData Data { get; }
	}
}