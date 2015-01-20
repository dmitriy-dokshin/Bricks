#region

using System;

using Bricks.Core.DateTime;

#endregion

namespace Bricks.Core.Impl.DateTime
{
	internal sealed class DateTimeProvider : IDateTimeProvider
	{
		private static readonly DateTimeOffset _unixStartTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

		#region Implementation of IDateTimeProvider

		DateTimeOffset IDateTimeProvider.Now
		{
			get
			{
				return DateTimeOffset.Now;
			}
		}

		public DateTimeOffset FromUnixTime(double unitTime)
		{
			return _unixStartTime.AddSeconds(unitTime);
		}

		public double ToUnixTime(DateTimeOffset dateTimeOffset)
		{
			return (dateTimeOffset.ToUniversalTime() - _unixStartTime).TotalSeconds;
		}

		#endregion
	}
}