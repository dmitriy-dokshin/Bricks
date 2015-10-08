#region

using System;

#endregion

namespace Bricks.WebAPI.Filters
{
	public interface IWebCacheController
	{
		void CacheUpdated(string cacheId);

		DateTime? GetCacheUpdatedAt(string cacheId);
	}
}