#region

using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Tasks
{
	public interface ITaskHelper
	{
		Task GetEmpty();

		Task<TResult> GetEmpty<TResult>();
	}
}