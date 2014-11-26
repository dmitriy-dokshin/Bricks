#region

using System;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Helpers
{
	public static class ExceptionExtensions
	{
		public static IResult<TResult> Catch<TResult>(this IExceptionHelper exceptionHelper, Func<TResult> func)
		{
			return exceptionHelper.Catch<TResult, Exception>(func);
		}
	}
}