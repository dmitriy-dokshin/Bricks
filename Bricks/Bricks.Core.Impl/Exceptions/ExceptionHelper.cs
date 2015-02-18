#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bricks.Core.Exceptions;
using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Impl.Exceptions
{
	/// <summary>
	/// Помощник работы с исключениями.
	/// </summary>
	internal sealed class ExceptionHelper : IExceptionHelper
	{
		private readonly IResultFactory _resultFactory;

		public ExceptionHelper(IResultFactory resultFactory)
		{
			_resultFactory = resultFactory;
		}

		#region Implementation of IExceptionHelper

		public string GetSummary(Exception exception, bool stackTrace = false)
		{
			StringBuilder summaryBuilder = new StringBuilder();
			summaryBuilder.Append(exception.GetType().Name);
			summaryBuilder.Append(": ");
			summaryBuilder.AppendLine(exception.Message);
			if (stackTrace)
			{
				summaryBuilder.AppendLine(exception.StackTrace);
			}

			return exception.Message;
		}

		public IResult<TResult> Catch<TResult>(Func<TResult> func, IReadOnlyCollection<Type> exceptionTypes, string message = null)
		{
			TResult result;
			try
			{
				result = func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return _resultFactory.CreateUnsuccessfulResult<TResult>(message, exception);
				}

				throw;
			}

			return _resultFactory.Create(result);
		}

		public IResult Catch(Action action, IReadOnlyCollection<Type> exceptionTypes, string message = null)
		{
			try
			{
				action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return _resultFactory.CreateUnsuccessfulResult(message, exception);
				}

				throw;
			}

			return _resultFactory.Create();
		}

		public TResult SimpleCatch<TResult>(Func<TResult> func, IReadOnlyCollection<Type> exceptionTypes)
		{
			try
			{
				return func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return default(TResult);
				}

				throw;
			}
		}

		public void SimpleCatch(Action action, IReadOnlyCollection<Type> exceptionTypes)
		{
			try
			{
				action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return;
				}

				throw;
			}
		}

		public async Task<IResult<TResult>> CatchAsync<TResult>(Func<Task<TResult>> func, IReadOnlyCollection<Type> exceptionTypes, string message = null)
		{
			TResult result;
			try
			{
				result = await func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return _resultFactory.CreateUnsuccessfulResult<TResult>(message, exception);
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return _resultFactory.CreateUnsuccessfulResult<TResult>(message, exception);
				}

				throw;
			}

			return _resultFactory.Create(result);
		}

		public async Task<IResult> CatchAsync(Func<Task> action, IReadOnlyCollection<Type> exceptionTypes, string message = null)
		{
			try
			{
				await action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return _resultFactory.CreateUnsuccessfulResult(message, exception);
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return _resultFactory.CreateUnsuccessfulResult(message, exception);
				}

				throw;
			}

			return _resultFactory.Create();
		}

		public async Task<TResult> SimpleCatchAsync<TResult>(Func<Task<TResult>> func, IReadOnlyCollection<Type> exceptionTypes)
		{
			try
			{
				return await func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return default(TResult);
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return default(TResult);
				}

				throw;
			}
		}

		public async Task SimpleCatchAsync(Func<Task> action, IReadOnlyCollection<Type> exceptionTypes)
		{
			try
			{
				await action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return;
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return;
				}

				throw;
			}
		}

		public async Task<IResult<TResult>> CatchAsync<TResult>(Task<TResult> task, IReadOnlyCollection<Type> exceptionTypes, string message = null)
		{
			TResult result;
			try
			{
				result = await task;
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return _resultFactory.CreateUnsuccessfulResult<TResult>(message, exception);
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return _resultFactory.CreateUnsuccessfulResult<TResult>(message, exception);
				}

				throw;
			}

			return _resultFactory.Create(result);
		}

		public async Task<IResult> CatchAsync(Task task, IReadOnlyCollection<Type> exceptionTypes, string message = null)
		{
			try
			{
				await task;
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return _resultFactory.CreateUnsuccessfulResult(message, exception);
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return _resultFactory.CreateUnsuccessfulResult(message, exception);
				}

				throw;
			}

			return _resultFactory.Create();
		}

		public async Task<TResult> SimpleCatchAsync<TResult>(Task<TResult> task, IReadOnlyCollection<Type> exceptionTypes)
		{
			try
			{
				return await task;
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return default(TResult);
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return default(TResult);
				}

				throw;
			}
		}

		public async Task SimpleCatchAsync(Task task, IReadOnlyCollection<Type> exceptionTypes)
		{
			try
			{
				await task;
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Any(x => x.IsInstanceOfType(exception)))
				{
					return;
				}

				var aggregateException = exception as AggregateException;
				if (aggregateException != null && aggregateException.InnerExceptions.All(e => exceptionTypes.Any(x => x.IsInstanceOfType(e))))
				{
					return;
				}

				throw;
			}
		}

		#endregion
	}
}