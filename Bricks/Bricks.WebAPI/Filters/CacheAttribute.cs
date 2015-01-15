#region

using System;

#endregion

namespace Bricks.WebAPI.Filters
{
	public sealed class CacheAttribute : Attribute
	{
		public CacheAttribute(double? serverLifetimeMinutes, double? clientLifetime)
		{
			ServerLifetime = serverLifetimeMinutes.HasValue ? TimeSpan.FromMinutes(serverLifetimeMinutes.Value) : (TimeSpan?)null;
			ClientLifetime = clientLifetime.HasValue ? TimeSpan.FromMinutes(clientLifetime.Value) : (TimeSpan?)null;
		}

		public TimeSpan? ServerLifetime { get; private set; }

		public TimeSpan? ClientLifetime { get; private set; }
	}
}