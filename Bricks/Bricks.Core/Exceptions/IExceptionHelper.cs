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
		string GetSummary(Exception exception, bool stackTrace = false);

		IResult<TResult> Catch<TResult>(Func<TResult> func, IReadOnlyCollection<Type> exceptionTypes, string message = null);

		IResult Catch(Action action, IReadOnlyCollection<Type> exceptionTypes, string message = null);

		TResult SimpleCatch<TResult>(Func<TResult> func, IReadOnlyCollection<Type> exceptionTypes);

		void SimpleCatch(Action action, IReadOnlyCollection<Type> exceptionTypes);

		#region Async

		Task<IResult<TResult>> CatchAsync<TResult>(Func<Task<TResult>> func, IReadOnlyCollection<Type> exceptionTypes, string message = null);

		Task<IResult> CatchAsync(Func<Task> action, IReadOnlyCollection<Type> exceptionTypes, string message = null);

		Task<TResult> SimpleCatchAsync<TResult>(Func<Task<TResult>> func, IReadOnlyCollection<Type> exceptionTypes);

		Task SimpleCatchAsync(Func<Task> action, IReadOnlyCollection<Type> exceptionTypes);

		Task<IResult<TResult>> CatchAsync<TResult>(Task<TResult> task, IReadOnlyCollection<Type> exceptionTypes, string message = null);

		Task<IResult> CatchAsync(Task task, IReadOnlyCollection<Type> exceptionTypes, string message = null);

		Task<TResult> SimpleCatchAsync<TResult>(Task<TResult> task, IReadOnlyCollection<Type> exceptionTypes);

		Task SimpleCatchAsync(Task task, IReadOnlyCollection<Type> exceptionTypes);

		#endregion
	}
}