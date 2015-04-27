#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Tasks
{
	public static class TaskExtensions
	{
		public static CancellationToken NoneIfNull(this CancellationToken? cancellationToken)
		{
			return cancellationToken ?? CancellationToken.None;
		}

		public async static Task<T> WithTimeOut<T>(this Task<T> task, TimeSpan timeout)
		{
			Task winner = await Task.WhenAny(task, Task.Delay(timeout));
			if (winner != task)
			{
				throw new TimeoutException();
			}

			return await task;
		}
	}
}