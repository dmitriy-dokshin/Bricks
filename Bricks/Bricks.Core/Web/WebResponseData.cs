namespace Bricks.Core.Web
{
	public sealed class WebResponseData<TResult, TErrorResult>
	{
		public WebResponseData(TResult result, TErrorResult errorResult)
		{
			Result = result;
			ErrorResult = errorResult;
		}

		public TResult Result { get; private set; }

		public TErrorResult ErrorResult { get; private set; }
	}
}