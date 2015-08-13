#region

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.EF.Linq
{
	public static class MultiQueryableExtensions
	{
		public static async Task<TSource[]> ToArrayAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.ToArrayAsync(cancellationToken)))).SelectMany(x => x).ToArray();
		}

		public static async Task<int> SumAsync(this IEnumerable<IQueryable<int>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<int?> SumAsync(this IEnumerable<IQueryable<int?>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<long> SumAsync(this IEnumerable<IQueryable<long>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<long?> SumAsync(this IEnumerable<IQueryable<long?>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<float> SumAsync(this IEnumerable<IQueryable<float>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<float?> SumAsync(this IEnumerable<IQueryable<float?>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<double> SumAsync(this IEnumerable<IQueryable<double>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<double?> SumAsync(this IEnumerable<IQueryable<double?>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<decimal> SumAsync(this IEnumerable<IQueryable<decimal>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<decimal?> SumAsync(this IEnumerable<IQueryable<decimal?>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<int> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<int?> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<long> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<long?> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<float> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<float?> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<double> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<double?> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<decimal> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<decimal?> SumAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.SumAsync(selector, cancellationToken)))).Sum(x => x);
		}

		public static async Task<int> CountAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.CountAsync(cancellationToken)))).Sum(x => x);
		}

		public static async Task<int> CountAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.CountAsync(predicate, cancellationToken)))).Sum(x => x);
		}

		public static IEnumerable<IQueryable<T>> Include<T, TProperty>(this IEnumerable<IQueryable<T>> source, Expression<Func<T, TProperty>> path)
		{
			return source.Select(q => q.Include(path));
		}

		public static async Task<TSource> FirstOrDefaultAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.FirstOrDefaultAsync(cancellationToken)))).FirstOrDefault(x => !Equals(x, default(TSource)));
		}

		public static async Task<TSource> FirstOrDefaultAsync<TSource>(this IEnumerable<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			return (await Task.WhenAll(source.Select(q => q.FirstOrDefaultAsync(predicate, cancellationToken)))).FirstOrDefault(x => !Equals(x, default(TSource)));
		}
	}
}