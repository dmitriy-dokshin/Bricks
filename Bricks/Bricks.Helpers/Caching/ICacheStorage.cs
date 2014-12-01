#region

using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Caching;

#endregion

namespace Bricks.Helpers.Caching
{
	public interface ICacheStorage
	{
		Task<ICacheManager> GetCacheManager(string name = null);
	}
}