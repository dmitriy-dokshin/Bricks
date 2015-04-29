#region

using System;
using System.Collections.Generic;
using System.Text;

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

		public static IResult GetInnerResult(this IResult result)
		{
			return result.InnerResult != null ? GetInnerResult(result.InnerResult) : result;
		}

		public static string GetMessage(this IResult result, bool exc = true, bool hierarchy = true)
		{
			StringBuilder messageBuilder = new StringBuilder();
			if (hierarchy)
			{
				foreach (var result1 in result.GetResultHierarchy())
				{
					AddResultMessage(exc, result1, messageBuilder);
				}
			}
			else
			{
				AddResultMessage(exc, result, messageBuilder);
			}

			string message = messageBuilder.ToString();
			return message;
		}

		private static void AddResultMessage(bool exc, IResult result, StringBuilder messageBuilder)
		{
			if (!string.IsNullOrEmpty(result.Message))
			{
				messageBuilder.AppendLine(result.Message);
			}

			if (exc && result.Exception != null)
			{
				foreach (var exception in result.Exception.GetExceptionHierarchy())
				{
					messageBuilder.AppendLine(exception.Message);
				}
			}
		}

		public static IEnumerable<IResult> GetResultHierarchy(this IResult result)
		{
			yield return result;
			if (result.InnerResult != null)
			{
				foreach (var innerResult in GetResultHierarchy(result.InnerResult))
				{
					yield return innerResult;
				}
			}
		}
	}
}