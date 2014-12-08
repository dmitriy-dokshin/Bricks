﻿#region

using System;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Exceptions
{
	public static class ExceptionExtensions
	{
		#region Func

		public static IResult<TResult> Catch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func, string message = null, params Type[] exceptionTypes)
		{
			return exceptionHelper.Catch(func, exceptionTypes, message);
		}

		public static TResult SimpleCatch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func, params Type[] exceptionTypes)
		{
			return exceptionHelper.SimpleCatch(func, exceptionTypes);
		}

		public static IResult<TResult> Catch<TResult, TException>(this IExceptionHelper exceptionHelper, Func<TResult> func, string message = null)
			where TException : Exception
		{
			return exceptionHelper.Catch(func, message, typeof(TException));
		}

		public static TResult SimpleCatch<TResult, TException>(this IExceptionHelper exceptionHelper, Func<TResult> func)
			where TException : Exception
		{
			return exceptionHelper.SimpleCatch(func, typeof(TException));
		}

		public static IResult<TResult> Catch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func, string message = null)
		{
			return exceptionHelper.Catch<TResult, Exception>(func, message);
		}

		public static TResult SimpleCatch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func)
		{
			return exceptionHelper.SimpleCatch<TResult, Exception>(func);
		}

		#endregion

		#region Async Func

		public static Task<IResult<TResult>> CatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func, string message = null, params Type[] exceptionTypes)
		{
			return exceptionHelper.CatchAsync(func, exceptionTypes, message);
		}

		public static Task<TResult> SimpleCatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func, params Type[] exceptionTypes)
		{
			return exceptionHelper.SimpleCatchAsync(func, exceptionTypes);
		}

		public static Task<IResult<TResult>> CatchAsync<TResult, TException>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func, string message = null)
			where TException : Exception
		{
			return exceptionHelper.CatchAsync(func, message, typeof(TException));
		}

		public static Task<TResult> SimpleCatchAsync<TResult, TException>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func)
			where TException : Exception
		{
			return exceptionHelper.SimpleCatchAsync(func, typeof(TException));
		}

		public static Task<IResult<TResult>> CatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func, string message = null)
		{
			return exceptionHelper.CatchAsync<TResult, Exception>(func, message);
		}

		public static Task<TResult> SimpleCatchAsync<TResult>(this IExceptionHelper exceptionHelper, Func<Task<TResult>> func)
		{
			return exceptionHelper.SimpleCatchAsync<TResult, Exception>(func);
		}

		#endregion

		#region Action

		public static IResult Catch(this IExceptionHelper exceptionHelper, Action action, string message = null, params Type[] exceptionTypes)
		{
			return exceptionHelper.Catch(action, exceptionTypes, message);
		}

		public static void SimpleCatch(this IExceptionHelper exceptionHelper, Action action, params Type[] exceptionTypes)
		{
			exceptionHelper.SimpleCatch(action, exceptionTypes);
		}

		public static IResult Catch<TException>(this IExceptionHelper exceptionHelper, Action action, string message = null)
			where TException : Exception
		{
			return exceptionHelper.Catch(action, message, typeof(TException));
		}

		public static void SimpleCatch<TException>(this IExceptionHelper exceptionHelper, Action action)
			where TException : Exception
		{
			exceptionHelper.SimpleCatch(action, typeof(TException));
		}

		public static IResult Catch(this IExceptionHelper exceptionHelper, Action action, string message = null)
		{
			return exceptionHelper.Catch<Exception>(action, message);
		}

		public static void SimpleCatch(this IExceptionHelper exceptionHelper, Action action)
		{
			exceptionHelper.SimpleCatch<Exception>(action);
		}

		#endregion

		#region Async Action

		public static Task<IResult> CatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action, string message = null, params Type[] exceptionTypes)
		{
			return exceptionHelper.CatchAsync(action, exceptionTypes, message);
		}

		public static Task SimpleCatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action, params Type[] exceptionTypes)
		{
			return exceptionHelper.SimpleCatchAsync(action, exceptionTypes);
		}

		public static Task<IResult> CatchAsync<TException>(this IExceptionHelper exceptionHelper, Func<Task> action, string message = null)
			where TException : Exception
		{
			return exceptionHelper.CatchAsync(action, message, typeof(TException));
		}

		public static Task SimpleCatchAsync<TException>(this IExceptionHelper exceptionHelper, Func<Task> action)
			where TException : Exception
		{
			return exceptionHelper.SimpleCatchAsync(action, typeof(TException));
		}

		public static Task<IResult> CatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action, string message = null)
		{
			return exceptionHelper.CatchAsync<Exception>(action, message);
		}

		public static Task SimpleCatchAsync(this IExceptionHelper exceptionHelper, Func<Task> action)
		{
			return exceptionHelper.SimpleCatchAsync<Exception>(action);
		}

		#endregion
	}
}