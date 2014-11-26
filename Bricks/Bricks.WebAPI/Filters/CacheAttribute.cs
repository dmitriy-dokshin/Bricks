#region

using System;

#endregion

namespace Bricks.WebAPI.Filters
{
	public sealed class CacheAttribute : Attribute
	{
		public CacheAttribute(double minutes)
		{
			Lifetime = TimeSpan.FromMinutes(minutes);
		}

		public TimeSpan Lifetime { get; private set; }
	}
}