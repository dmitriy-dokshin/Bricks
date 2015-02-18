namespace Bricks.Core.Results
{
	public interface IResultHelper
	{
		string GetSummary(IResult result, bool stackTrace = false, bool exception = false, bool exceptionStackTrace = false);
	}
}