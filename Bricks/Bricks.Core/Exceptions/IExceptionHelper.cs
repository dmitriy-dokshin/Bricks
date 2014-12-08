#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Exceptions
{
	public interface IExceptionHelper
	{
		IResult<TResult> Catch<TResult>(Func<TResult> func, IEnumerable<Type> exceptionTypes, string message = null);

		IResult Catch(Action action, IEnumerable<Type> exceptionTypes, string message = null);

		TResult SimpleCatch<TResult>(Func<TResult> func, IEnumerable<Type> exceptionTypes);

		void SimpleCatch(Action action, IEnumerable<Type> exceptionTypes);

		#region Async

		Task<IResult<TResult>> CatchAsync<TResult>(Func<Task<TResult>> func, IEnumerable<Type> exceptionTypes, string message = null);

		Task<IResult> CatchAsync(Func<Task> action, IEnumerable<Type> exceptionTypes, string message = null);

		Task<TResult> SimpleCatchAsync<TResult>(Func<Task<TResult>> func, IEnumerable<Type> exceptionTypes);

		Task SimpleCatchAsync(Func<Task> action, IEnumerable<Type> exceptionTypes);

		#endregion
	}
}