#region

using System;

#endregion

namespace Bricks.WebAPI.Filters
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class CacheAttribute : Attribute
	{
		public double ServerLifetime { get; set; }

		public double ClientLifetime { get; set; }

		public string CacheManagerKey { get; set; }
	}
}