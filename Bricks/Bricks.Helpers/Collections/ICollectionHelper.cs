#region

using System.Collections.Generic;

#endregion

namespace Bricks.Helpers.Collections
{
	public interface ICollectionHelper
	{
		ICollection<T> GetEmptyCollection<T>();

		IReadOnlyCollection<T> GetEmptyReadOnlyCollection<T>();
	}
}