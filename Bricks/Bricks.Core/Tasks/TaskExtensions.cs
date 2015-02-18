#region

using System.Threading;

#endregion

namespace Bricks.Core.Tasks
{
	public static class TaskExtensions
	{
		public static CancellationToken NoneIfNull(this CancellationToken? cancellationToken)
		{
			return cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None;
		}
	}
}