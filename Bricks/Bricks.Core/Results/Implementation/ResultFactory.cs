#region

using System;

#endregion

namespace Bricks.Core.Results.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="ResultFactory" />.
	/// </summary>
	internal sealed class ResultFactory : IResultFactory
	{
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
			return new Result(success, message, exception, innerResult);
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
			return new Result<TData>(data, success, message, exception, innerResult);
		}

		#endregion

		private class Result : IResult
		{
			private readonly string _message;

			public Result(bool success, string message, Exception exception, IResult innerResult)
			{
				InnerResult = innerResult;
				Success = success;
				_message = message;
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
			public string Message
			{
				get
				{
					string message = _message;
					if (message == null && InnerResult != null)
					{
						message = InnerResult.Message;
					}

					return message;
				}
			}

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
	}
}