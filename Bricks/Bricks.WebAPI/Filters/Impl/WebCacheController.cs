#region

using System;
using System.Collections.Immutable;

using Bricks.Core.Sync;

#endregion

namespace Bricks.WebAPI.Filters.Impl
{
	internal sealed class WebCacheController : IWebCacheController
	{
		private readonly IInterlockedHelper _interlockedHelper;
		private IImmutableDictionary<string, DateTime> _cacheUpdatedAtDict;

		public WebCacheController(IInterlockedHelper interlockedHelper)
		{
			_interlockedHelper = interlockedHelper;
			_cacheUpdatedAtDict = ImmutableDictionary.Create<string, DateTime>();
		}

		#region Implementation of IWebCacheController

		public void CacheUpdated(string cacheId)
		{
			_interlockedHelper.CompareExchange(ref _cacheUpdatedAtDict, x => x.SetItem(cacheId, DateTime.UtcNow));
		}

		public DateTime? GetCacheUpdatedAt(string cacheId)
		{
			DateTime updatedAt;
			if (_cacheUpdatedAtDict.TryGetValue(cacheId, out updatedAt))
			{
				return updatedAt;
			}

			return null;
		}

		#endregion
	}
}