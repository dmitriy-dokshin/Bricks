#region

using System;

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

		public static IResult GetInnerResult(this IResult result)
		{
			return result.InnerResult != null ? GetInnerResult(result.InnerResult) : result;
		}

		public static string GetMessage(this IResult result, bool innerResult = true, bool exception = true)
		{
			string message = result.Message;
			if (string.IsNullOrEmpty(message) && innerResult && result.InnerResult != null)
			{
				message = GetMessage(result.InnerResult);
			}

			if (string.IsNullOrEmpty(message) && exception && result.Exception != null)
			{
				message = result.Exception.Message;
			}

			return message;
		}
	}
}