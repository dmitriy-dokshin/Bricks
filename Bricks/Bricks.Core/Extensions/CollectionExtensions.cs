#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Bricks.Core.Extensions
{
	public static class CollectionExtensions
	{
		public static T[] ToArrayIfNot<T>(this IEnumerable<T> items)
		{
			return items as T[] ?? items.ToArray();
		}
	}
}