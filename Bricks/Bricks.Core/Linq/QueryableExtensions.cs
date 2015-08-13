#region

using System.Linq;

#endregion

namespace Bricks.Core.Linq
{
	public static class QueryableExtensions
	{
		public static IQueryable<TSource> Skip<TSource>(this IQueryable<TSource> source, int? count)
		{
			return count.HasValue ? Queryable.Skip(source, count.Value) : source;
		}

		public static IQueryable<TSource> Take<TSource>(this IQueryable<TSource> source, int? count)
		{
			return count.HasValue ? Queryable.Take(source, count.Value) : source;
		}
	}
}