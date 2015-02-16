#region

using System;

using Bricks.Core.Exceptions;

#endregion

namespace Bricks.Core.Results
{
	public static class ResultExtensions
	{
		public static IResult<TData> CreateUnsuccessfulResult<TData>(this IResultFactory resultFactory, string message = null, Exception exception = null, IResult innerResult = null)
		{
			return resultFactory.Create(default(TData), false, message, exception, innerResult);
		}

		public static IResult CreateUnsuccessfulResult(this IResultFactory resultFactory, string message = null, Exception exception = null, IResult innerResult = null)
		{
			return resultFactory.Create(false, message, exception, innerResult);
		}

		public static Exception GetException(this IResult result)
		{
			return result.Exception ?? (result.InnerResult != null ? GetException(result.InnerResult) : null);
		}

		public static Exception GetInnerException(this IResult result)
		{
			Exception exception = result.GetException();
			return exception != null ? exception.GetInnerException() : null;
		}
	}
}