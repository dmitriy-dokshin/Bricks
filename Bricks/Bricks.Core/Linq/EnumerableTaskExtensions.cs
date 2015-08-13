#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Linq
{
	public static class EnumerableTaskExtensions
	{
		#region Where

		public static Task<IEnumerable<TSource>> Where<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
		{
			return source.ContinueWith(x => x.Result.Where(predicate));
		}

		public static Task<IEnumerable<TSource>> Where<TSource>(this Task<IOrderedEnumerable<TSource>> source, Func<TSource, bool> predicate)
		{
			return source.ContinueWith(x => x.Result.Where(predicate));
		}

		public static Task<IEnumerable<TSource>> Where<TSource>(this Task<TSource[]> source, Func<TSource, bool> predicate)
		{
			return source.ContinueWith(x => x.Result.Where(predicate));
		}

		#endregion

		#region Select

		public static Task<IEnumerable<TResult>> Select<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TResult> selector)
		{
			return source.ContinueWith(x => x.Result.Select(selector));
		}

		public static Task<IEnumerable<TResult>> Select<TSource, TResult>(this Task<IOrderedEnumerable<TSource>> source, Func<TSource, TResult> selector)
		{
			return source.ContinueWith(x => x.Result.Select(selector));
		}

		public static Task<IEnumerable<TResult>> Select<TSource, TResult>(this Task<TSource[]> source, Func<TSource, TResult> selector)
		{
			return source.ContinueWith(x => x.Result.Select(selector));
		}

		#endregion

		#region SelectMany

		public static Task<IEnumerable<TResult>> SelectMany<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			return source.ContinueWith(x => x.Result.SelectMany(selector));
		}

		public static Task<IEnumerable<TResult>> SelectMany<TSource, TResult>(this Task<IOrderedEnumerable<TSource>> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			return source.ContinueWith(x => x.Result.SelectMany(selector));
		}

		public static Task<IEnumerable<TResult>> SelectMany<TSource, TResult>(this Task<TSource[]> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			return source.ContinueWith(x => x.Result.SelectMany(selector));
		}

		#endregion

		#region Take

		public static Task<IEnumerable<TSource>> Take<TSource>(this Task<IEnumerable<TSource>> source, int count)
		{
			return source.ContinueWith(x => x.Result.Take(count));
		}

		public static Task<IEnumerable<TSource>> Take<TSource>(this Task<IOrderedEnumerable<TSource>> source, int count)
		{
			return source.ContinueWith(x => x.Result.Take(count));
		}

		public static Task<IEnumerable<TSource>> Take<TSource>(this Task<TSource[]> source, int count)
		{
			return source.ContinueWith(x => x.Result.Take(count));
		}

		public static Task<IEnumerable<TSource>> Take<TSource>(this Task<IEnumerable<TSource>> source, int? count)
		{
			return count.HasValue ? source.Take(count.Value) : source;
		}

		public static Task<IEnumerable<TSource>> Take<TSource>(this Task<IOrderedEnumerable<TSource>> source, int? count)
		{
			return count.HasValue ? source.Take(count.Value) : source.ContinueWith(x => (IEnumerable<TSource>)x.Result);
		}

		public static Task<IEnumerable<TSource>> Take<TSource>(this Task<TSource[]> source, int? count)
		{
			return count.HasValue ? source.Take(count.Value) : source.ContinueWith(x => (IEnumerable<TSource>)x.Result);
		}

		#endregion

		#region Skip

		public static Task<IEnumerable<TSource>> Skip<TSource>(this Task<IEnumerable<TSource>> source, int count)
		{
			return source.ContinueWith(x => x.Result.Skip(count));
		}

		public static Task<IEnumerable<TSource>> Skip<TSource>(this Task<IOrderedEnumerable<TSource>> source, int count)
		{
			return source.ContinueWith(x => x.Result.Skip(count));
		}

		public static Task<IEnumerable<TSource>> Skip<TSource>(this Task<TSource[]> source, int count)
		{
			return source.ContinueWith(x => x.Result.Skip(count));
		}

		public static Task<IEnumerable<TSource>> Skip<TSource>(this Task<IEnumerable<TSource>> source, int? count)
		{
			return count.HasValue ? source.Skip(count.Value) : source;
		}

		public static Task<IEnumerable<TSource>> Skip<TSource>(this Task<IOrderedEnumerable<TSource>> source, int? count)
		{
			return count.HasValue ? source.Skip(count.Value) : source.ContinueWith(x => (IEnumerable<TSource>) x.Result);
		}

		public static Task<IEnumerable<TSource>> Skip<TSource>(this Task<TSource[]> source, int? count)
		{
			return count.HasValue ? source.Skip(count.Value) : source.ContinueWith(x => (IEnumerable<TSource>) x.Result);
		}

		#endregion

		#region OrderBy

		public static Task<IOrderedEnumerable<TSource>> OrderBy<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.OrderBy(keySelector));
		}

		public static Task<IOrderedEnumerable<TSource>> OrderBy<TSource, TKey>(this Task<IOrderedEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.OrderBy(keySelector));
		}

		public static Task<IOrderedEnumerable<TSource>> OrderBy<TSource, TKey>(this Task<TSource[]> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.OrderBy(keySelector));
		}

		#endregion

		#region OrderByDescending

		public static Task<IOrderedEnumerable<TSource>> OrderByDescending<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.OrderByDescending(keySelector));
		}

		public static Task<IOrderedEnumerable<TSource>> OrderByDescending<TSource, TKey>(this Task<IOrderedEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.OrderByDescending(keySelector));
		}

		public static Task<IOrderedEnumerable<TSource>> OrderByDescending<TSource, TKey>(this Task<TSource[]> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.OrderByDescending(keySelector));
		}

		#endregion

		#region GroupBy

		public static Task<IEnumerable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.GroupBy(keySelector));
		}

		public static Task<IEnumerable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Task<IOrderedEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.GroupBy(keySelector));
		}

		public static Task<IEnumerable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Task<TSource[]> source, Func<TSource, TKey> keySelector)
		{
			return source.ContinueWith(x => x.Result.GroupBy(keySelector));
		}

		#endregion

		#region Distinct

		public static Task<IEnumerable<TSource>> Distinct<TSource>(this Task<IEnumerable<TSource>> source)
		{
			return source.ContinueWith(x => x.Result.Distinct());
		}

		public static Task<IEnumerable<TSource>> Distinct<TSource>(this Task<IOrderedEnumerable<TSource>> source)
		{
			return source.ContinueWith(x => x.Result.Distinct());
		}

		public static Task<IEnumerable<TSource>> Distinct<TSource>(this Task<TSource[]> source)
		{
			return source.ContinueWith(x => x.Result.Distinct());
		}

		#endregion

		#region Count

		public static Task<int> Count<TSource>(this Task<IEnumerable<TSource>> source)
		{
			return source.ContinueWith(x => x.Result.Count());
		}

		public static Task<int> Count<TSource>(this Task<IOrderedEnumerable<TSource>> source)
		{
			return source.ContinueWith(x => x.Result.Count());
		}

		public static Task<int> Count<TSource>(this Task<TSource[]> source)
		{
			return source.ContinueWith(x => x.Result.Count());
		}

		#endregion

		#region ToArray

		public static Task<TSource[]> ToArray<TSource>(this Task<IEnumerable<TSource>> source)
		{
			return source.ContinueWith(x => x.Result.ToArray());
		}

		public static Task<TSource[]> ToArray<TSource>(this Task<IOrderedEnumerable<TSource>> source)
		{
			return source.ContinueWith(x => x.Result.ToArray());
		}

		#endregion
	}
}