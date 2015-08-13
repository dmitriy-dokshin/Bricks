#region

using System;

#endregion

namespace Bricks.Core.DateTime
{
	public static class DateTimeHelper
	{
		private static readonly System.DateTime _unixStartTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static System.DateTime FromUnixTime(double unixTime)
		{
			return _unixStartTime.AddSeconds(unixTime);
		}

		public static double ToUnixTime(System.DateTime dateTime)
		{
			return (dateTime.ToUniversalTime() - _unixStartTime).TotalSeconds;
		}

		public static double ToUnixTime(DateTimeOffset dateTime)
		{
			return (dateTime.ToUniversalTime() - _unixStartTime).TotalSeconds;
		}
	}
}