#region

using System;
using System.Collections.Generic;
using System.Text;

using Bricks.Core.Exceptions;
using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Impl.Results
{
	internal sealed class ResultHelper : IResultHelper
	{
		private readonly IExceptionHelper _exceptionHelper;

		public ResultHelper(IExceptionHelper exceptionHelper)
		{
			_exceptionHelper = exceptionHelper;
		}

		#region Implementation of IResultHelper

		public string GetSummary(IResult result, bool stackTrace = false, bool exception = false, bool exceptionStackTrace = false, bool hierarchy = true)
		{
			var summaryBuilder = new StringBuilder();
			if (hierarchy)
			{
				foreach (var result1 in result.GetResultHierarchy())
				{
					AddResultSummary(result1, stackTrace, exception, exceptionStackTrace, summaryBuilder);
				}
			}
			else
			{
				AddResultSummary(result, stackTrace, exception, exceptionStackTrace, summaryBuilder);
			}
			return summaryBuilder.ToString();
		}

		private void AddResultSummary(IResult result, bool stackTrace, bool exception, bool exceptionStackTrace, StringBuilder summaryBuilder)
		{
			if (!string.IsNullOrEmpty(result.Message))
			{
				summaryBuilder.AppendLine(result.Message);
			}

			if (stackTrace)
			{
				summaryBuilder.AppendLine(System.Environment.StackTrace);
			}

			if (exception && result.Exception != null)
			{
				IEnumerable<Exception> exceptionHierarchy = result.Exception.GetExceptionHierarchy();
				foreach (Exception e in exceptionHierarchy)
				{
					summaryBuilder.AppendLine(_exceptionHelper.GetSummary(e, exceptionStackTrace));
				}
			}
		}

		#endregion
	}
}