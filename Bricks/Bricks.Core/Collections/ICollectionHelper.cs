#region

using System.Collections.Generic;

#endregion

namespace Bricks.Core.Collections
{
	public interface ICollectionHelper
	{
		ICollection<T> GetEmptyCollection<T>();

		IReadOnlyCollection<T> GetEmptyReadOnlyCollection<T>();

		IEnumerable<T> Single<T>(T item);
	}
}