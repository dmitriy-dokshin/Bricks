#region

using System;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Exceptions
{
	public static class ExceptionExtensions
	{
		#region Func

		public static IResult<TResult> Catch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func, params Type[] exceptionTypes)
		{
			return exceptionHelper.Catch(func, exceptionTypes);
		}

		public static TResult SimpleCatch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func, params Type[] exceptionTypes)
		{
			return exceptionHelper.SimpleCatch(func, exceptionTypes);
		}

		public static IResult<TResult> Catch<TResult, TException>(this IExceptionHelper exceptionHelper, Func<TResult> func)
			where TException : Exception
		{
			return exceptionHelper.Catch(func, typeof(TException));
		}

		public static TResult SimpleCatch<TResult, TException>(this IExceptionHelper exceptionHelper, Func<TResult> func)
			where TException : Exception
		{
			return exceptionHelper.SimpleCatch(func, typeof(TException));
		}

		public static IResult<TResult> Catch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func)
		{
			return exceptionHelper.Catch<TResult, Exception>(func);
		}

		public static TResult SimpleCatch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func)
		{
			return exceptionHelper.SimpleCatch<TResult, Exception>(func);
		}

		#endregion

		#region Async Func

		public static Task<IResult<TResult>> CatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func, params Type[] exceptionTypes)
		{
			return exceptionHelper.CatchAsync(func, exceptionTypes);
		}

		public static Task<TResult> SimpleCatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func, params Type[] exceptionTypes)
		{
			return exceptionHelper.SimpleCatchAsync(func, exceptionTypes);
		}

		public static Task<IResult<TResult>> CatchAsync<TResult, TException>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func)
			where TException : Exception
		{
			return exceptionHelper.CatchAsync(func, typeof(TException));
		}

		public static Task<TResult> SimpleCatchAsync<TResult, TException>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func)
			where TException : Exception
		{
			return exceptionHelper.SimpleCatchAsync(func, typeof(TException));
		}

		public static Task<IResult<TResult>> CatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func)
		{
			return exceptionHelper.CatchAsync<TResult, Exception>(func);
		}

		public static Task<TResult> SimpleCatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func)
		{
			return exceptionHelper.SimpleCatchAsync<TResult, Exception>(func);
		}

		#endregion

		#region Action

		public static IResult Catch(this IExceptionHelper exceptionHelper, Action action, params Type[] exceptionTypes)
		{
			return exceptionHelper.Catch(action, exceptionTypes);
		}

		public static void SimpleCatch(this IExceptionHelper exceptionHelper, Action action, params Type[] exceptionTypes)
		{
			exceptionHelper.SimpleCatch(action, exceptionTypes);
		}

		public static IResult Catch<TException>(this IExceptionHelper exceptionHelper, Action action)
			where TException : Exception
		{
			return exceptionHelper.Catch(action, typeof(TException));
		}

		public static void SimpleCatch<TException>(this IExceptionHelper exceptionHelper, Action action)
			where TException : Exception
		{
			exceptionHelper.SimpleCatch(action, typeof(TException));
		}

		public static IResult Catch(this IExceptionHelper exceptionHelper, Action action)
		{
			return exceptionHelper.Catch<Exception>(action);
		}

		public static void SimpleCatch(this IExceptionHelper exceptionHelper, Action action)
		{
			exceptionHelper.SimpleCatch<Exception>(action);
		}

		#endregion

		#region Async Action

		public static Task<IResult> CatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action, params Type[] exceptionTypes)
		{
			return exceptionHelper.CatchAsync(action, exceptionTypes);
		}

		public static Task SimpleCatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action, params Type[] exceptionTypes)
		{
			return exceptionHelper.SimpleCatchAsync(action, exceptionTypes);
		}

		public static Task<IResult> CatchAsync<TException>(this IExceptionHelper exceptionHelper, Func<Task> action)
			where TException : Exception
		{
			return exceptionHelper.CatchAsync(action, typeof(TException));
		}

		public static Task SimpleCatchAsync<TException>(this IExceptionHelper exceptionHelper, Func<Task> action)
			where TException : Exception
		{
			return exceptionHelper.SimpleCatchAsync(action, typeof(TException));
		}

		public static Task<IResult> CatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action)
		{
			return exceptionHelper.CatchAsync<Exception>(action);
		}

		public static Task SimpleCatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action)
		{
			return exceptionHelper.SimpleCatchAsync<Exception>(action);
		}

		#endregion
	}
}