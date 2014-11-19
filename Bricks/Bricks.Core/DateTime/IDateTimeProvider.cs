#region

using System;

#endregion

namespace Bricks.Core.DateTime
{
	public interface IDateTimeProvider
	{
		DateTimeOffset Now { get; }
	}
}