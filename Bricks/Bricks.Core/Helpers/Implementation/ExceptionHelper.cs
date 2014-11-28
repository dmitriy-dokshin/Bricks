#region

using System;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Helpers.Implementation
{
	/// <summary>
	/// Помощник работы с исключениями.
	/// </summary>
	internal sealed class ExceptionHelper : IExceptionHelper
	{
		private readonly IResultFactory _resultFactory;

		public ExceptionHelper(IResultFactory resultFactory)
		{
			_resultFactory = resultFactory;
		}

		#region Implementation of IExceptionHelper

		public IResult<TResult> Catch<TResult, TException>(Func<TResult> func) where TException : Exception
		{
			try
			{
				TResult data = func();
				return _resultFactory.Create(data);
			}
			catch (TException exception)
			{
				return _resultFactory.CreateUnsuccessfulResult<TResult>(exception: exception);
			}
		}

		public TResult SimpleCatch<TResult, TException>(Func<TResult> func) where TException : Exception
		{
			try
			{
				return func();
			}
			catch (TException exception)
			{
				return default(TResult);
			}
		}

		#endregion
	}
}