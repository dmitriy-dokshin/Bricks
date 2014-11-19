#region

using System;

#endregion

namespace Bricks.Core.DateTime.Implementation
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