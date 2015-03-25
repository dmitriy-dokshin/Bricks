#region

using System.Collections.Generic;

#endregion

namespace Bricks.Core.Collections
{
	public interface ICollectionHelper
	{
		ICollection<T> GetEmptyCollection<T>();

		IList<T> GetEmptyList<T>();

		IReadOnlyCollection<T> GetEmptyReadOnlyCollection<T>();

		IReadOnlyList<T> GetEmptyReadOnlyList<T>();

		IEnumerable<T> Single<T>(T item);
	}
}