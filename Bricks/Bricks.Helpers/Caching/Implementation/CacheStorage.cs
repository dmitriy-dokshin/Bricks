#region

using System.Collections.Immutable;
using System.Threading.Tasks;

using Bricks.Helpers.Sync;
using Bricks.Sync;

using Microsoft.Practices.EnterpriseLibrary.Caching;

#endregion

namespace Bricks.Helpers.Caching.Implementation
{
	internal sealed class CacheStorage : ICacheStorage
	{
		private readonly IInterlockedHelper _interlockedHelper;
		private readonly ILockStorage _lockStorage;
		private IImmutableDictionary<string, ICacheManager> _cacheManagers;

		public CacheStorage(ILockStorage lockStorage, IInterlockedHelper interlockedHelper)
		{
			_lockStorage = lockStorage;
			_interlockedHelper = interlockedHelper;
			_cacheManagers = ImmutableDictionary.Create<string, ICacheManager>();
		}

		#region Implementation of ICacheStorage

		public async Task<ICacheManager> GetCacheManager(string name = null)
		{
			if (name == null)
			{
				name = string.Empty;
			}

			ICacheManager cacheManager;
			if (!_cacheManagers.TryGetValue(name, out cacheManager))
			{
				ILockContainer lockContainer;
				using (_lockStorage.GetContainer(name, out lockContainer))
				{
					ILockAsync @lock;
					using (lockContainer.GetLock(name, out @lock))
					{
						using (await @lock.Enter())
						{
							if (!_cacheManagers.TryGetValue(name, out cacheManager))
							{
								cacheManager = string.IsNullOrEmpty(name) ? CacheFactory.GetCacheManager() : CacheFactory.GetCacheManager(name);
								_interlockedHelper.CompareExchange(ref _cacheManagers, x => x.Add(name, cacheManager));
							}
						}
					}
				}
			}

			return cacheManager;
		}

		#endregion
	}
}