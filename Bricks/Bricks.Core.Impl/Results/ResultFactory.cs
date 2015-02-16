#region

using System;
using System.Diagnostics;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Impl.Results
{
	/// <summary>
	/// Реализация по умолчанию <see cref="ResultFactory" />.
	/// </summary>
	internal sealed class ResultFactory : IResultFactory
	{
		private class Result : IResult
		{
			public Result(bool success, string message, Exception exception, IResult innerResult)
			{
				InnerResult = innerResult;
				Success = success;
				Message = message;
				Exception = exception;
			}

			#region Implementation of IResult

			/// <summary>
			/// Признак успешного завершения.
			/// </summary>
			public bool Success { get; private set; }

			/// <summary>
			/// Сообщение, описывающее результат.
			/// </summary>
			public string Message { get; private set; }

			/// <summary>
			/// Исключение.
			/// </summary>
			public Exception Exception { get; private set; }

			/// <summary>
			/// Внутренний результат.
			/// </summary>
			public IResult InnerResult { get; private set; }

			#endregion
		}

		private sealed class Result<TData> : Result, IResult<TData>
		{
			public Result(TData data, bool success, string message, Exception exception, IResult innerResult)
				: base(success, message, exception, innerResult)
			{
				Data = data;
			}

			#region Implementation of IResult<out TData>

			/// <summary>
			/// Данные.
			/// </summary>
			public TData Data { get; private set; }

			#endregion
		}

		#region Implementation of IResultFactory

		/// <summary>
		/// Создаёт результат выполнения функции.
		/// </summary>
		/// <param name="success">Признак успешного завершения.</param>
		/// <param name="message">Сообщение, описывающее результат.</param>
		/// <param name="exception">Исключение.</param>
		/// <param name="innerResult">Внутренний результат.</param>
		/// <returns>Результат выполнения функции.</returns>
		public IResult Create(bool success = true, string message = null, Exception exception = null, IResult innerResult = null)
		{
			var result = new Result(success, message, exception, innerResult);
			if (!result.Success && (!string.IsNullOrEmpty(result.Message)))
			{
				Trace.TraceWarning(result.Message);
			}

			return result;
		}

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
		public IResult<TData> Create<TData>(TData data = default (TData), bool success = true, string message = null, Exception exception = null, IResult innerResult = null)
		{
			var result = new Result<TData>(data, success, message, exception, innerResult);
			if (!result.Success && (!string.IsNullOrEmpty(result.Message)))
			{
				Trace.TraceWarning(result.Message);
			}

			return result;
		}

		#endregion
	}
}