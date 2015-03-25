#region

using System;
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

		public static int IndexOf<T>(this IReadOnlyList<T> items, Func<T, bool> predicate)
		{
			int index = -1;
			for (var i = 0; i < items.Count; i++)
			{
				if (predicate(items[i]))
				{
					index = i;
					break;
				}
			}

			return index;
		}
	}
}