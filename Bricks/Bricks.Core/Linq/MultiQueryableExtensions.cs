#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Bricks.Core.Linq
{
	public static class MultiQueryableExtensions
	{
		public static IEnumerable<IQueryable<TSource>> Where<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate)
		{
			return source.Select(q => q.Where(predicate));
		}

		public static IEnumerable<IQueryable<TResult>> Select<TSource, TResult>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, TResult>> selector)
		{
			return source.Select(q => q.Select(selector));
		}

		public static IEnumerable<IQueryable<TResult>> SelectMany<TSource, TResult>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
		{
			return source.Select(q => q.SelectMany(selector));
		}

		public static IEnumerable<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector)
		{
			return source.Select(q => q.OrderBy(keySelector));
		}

		public static IEnumerable<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector)
		{
			return source.Select(q => q.OrderByDescending(keySelector));
		}

		public static IEnumerable<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this IEnumerable<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector)
		{
			return source.Select(x => x.ThenBy(keySelector));
		}

		public static IEnumerable<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this IEnumerable<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector)
		{
			return source.Select(q => q.ThenByDescending(keySelector));
		}

		public static IEnumerable<IQueryable<TSource>> Take<TSource>(this IEnumerable<IQueryable<TSource>> source, int count)
		{
			return source.Select(q => q.Take(count));
		}

		public static IEnumerable<IQueryable<TSource>> Take<TSource>(this IEnumerable<IQueryable<TSource>> source, int? count)
		{
			return count.HasValue ? source.Select(q => q.Take(count.Value)) : source;
		}

		public static IEnumerable<IQueryable<TSource>> Skip<TSource>(this IEnumerable<IQueryable<TSource>> source, int count)
		{
			return source.Select(q => q.Skip(count));
		}

		public static IEnumerable<IQueryable<TSource>> Skip<TSource>(this IEnumerable<IQueryable<TSource>> source, int? count)
		{
			return count.HasValue ? source.Select(q => q.Skip(count.Value)) : source;
		}

		public static IEnumerable<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector)
		{
			return source.Select(q => q.GroupBy(keySelector));
		}

		public static IEnumerable<IQueryable<TSource>> Distinct<TSource>(this IEnumerable<IQueryable<TSource>> source)
		{
			return source.Select(q => q.Distinct());
		}
	}
}