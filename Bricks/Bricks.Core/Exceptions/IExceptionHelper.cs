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
		IResult<TResult> Catch<TResult>(Func<TResult> func, IEnumerable<Type> exceptionTypes);

		IResult Catch(Action action, IEnumerable<Type> exceptionTypes);

		TResult SimpleCatch<TResult>(Func<TResult> func, IEnumerable<Type> exceptionTypes);

		void SimpleCatch(Action action, IEnumerable<Type> exceptionTypes);

		#region Async

		Task<IResult<TResult>> CatchAsync<TResult>(Func<Task<TResult>> func, IEnumerable<Type> exceptionTypes);

		Task<IResult> CatchAsync(Func<Task> action, IEnumerable<Type> exceptionTypes);

		Task<TResult> SimpleCatchAsync<TResult>(Func<Task<TResult>> func, IEnumerable<Type> exceptionTypes);

		Task SimpleCatchAsync(Func<Task> action, IEnumerable<Type> exceptionTypes);

		#endregion
	}
}