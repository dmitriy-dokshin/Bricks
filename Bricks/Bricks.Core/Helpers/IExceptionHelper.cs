#region

using System;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Helpers
{
	public interface IExceptionHelper
	{
		IResult<TResult> Catch<TResult, TException>(Func<TResult> func)
			where TException : Exception;
	}
}