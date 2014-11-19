#region

using System;

#endregion

namespace Bricks.Core.Results
{
	public static class ResultExtensions
	{
		public static IResult<TData> CreateUnsuccessfulResult<TData>(this IResultFactory resultFactory, string message = null, Exception exception = null, IResult inneResult = null)
		{
			return resultFactory.Create(default(TData), false, message, exception, inneResult);
		}
	}
}