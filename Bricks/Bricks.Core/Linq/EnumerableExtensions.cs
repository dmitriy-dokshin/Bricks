#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Bricks.Core.Linq
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int? count)
		{
			return count.HasValue ? Enumerable.Skip(source, count.Value) : source;
		}

		public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int? count)
		{
			return count.HasValue ? Enumerable.Take(source, count.Value) : source;
		}
	}
}