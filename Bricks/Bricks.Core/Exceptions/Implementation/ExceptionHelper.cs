#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Exceptions.Implementation
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

		public IResult<TResult> Catch<TResult>(Func<TResult> func, IEnumerable<Type> exceptionTypes, string message = null)
		{
			TResult result;
			try
			{
				result = func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return _resultFactory.CreateUnsuccessfulResult<TResult>(message, exception);
				}

				throw;
			}

			return _resultFactory.Create(result);
		}

		public IResult Catch(Action action, IEnumerable<Type> exceptionTypes, string message = null)
		{
			try
			{
				action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return _resultFactory.CreateUnsuccessfulResult(message, exception);
				}

				throw;
			}

			return _resultFactory.Create();
		}

		public TResult SimpleCatch<TResult>(Func<TResult> func, IEnumerable<Type> exceptionTypes)
		{
			try
			{
				return func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return default(TResult);
				}

				throw;
			}
		}

		public void SimpleCatch(Action action, IEnumerable<Type> exceptionTypes)
		{
			try
			{
				action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return;
				}

				throw;
			}
		}

		public async Task<IResult<TResult>> CatchAsync<TResult>(Func<Task<TResult>> func, IEnumerable<Type> exceptionTypes, string message = null)
		{
			TResult result;
			try
			{
				result = await func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return _resultFactory.CreateUnsuccessfulResult<TResult>(message, exception);
				}

				throw;
			}

			return _resultFactory.Create(result);
		}

		public async Task<IResult> CatchAsync(Func<Task> action, IEnumerable<Type> exceptionTypes, string message = null)
		{
			try
			{
				await action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return _resultFactory.CreateUnsuccessfulResult(message, exception);
				}

				throw;
			}

			return _resultFactory.Create();
		}

		public async Task<TResult> SimpleCatchAsync<TResult>(Func<Task<TResult>> func, IEnumerable<Type> exceptionTypes)
		{
			try
			{
				return await func();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return default(TResult);
				}

				throw;
			}
		}

		public async Task SimpleCatchAsync(Func<Task> action, IEnumerable<Type> exceptionTypes)
		{
			try
			{
				await action();
			}
			catch (Exception exception)
			{
				if (exceptionTypes.Contains(exception.GetType()))
				{
					return;
				}

				throw;
			}
		}

		#endregion
	}
}