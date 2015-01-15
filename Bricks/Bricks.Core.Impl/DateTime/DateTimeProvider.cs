#region

using System;

using Bricks.Core.DateTime;

#endregion

namespace Bricks.Core.Impl.DateTime
{
	internal sealed class DateTimeProvider : IDateTimeProvider
	{
		#region Implementation of IDateTimeProvider

		DateTimeOffset IDateTimeProvider.Now
		{
			get
			{
				return DateTimeOffset.Now;
			}
		}

		#endregion
	}
}