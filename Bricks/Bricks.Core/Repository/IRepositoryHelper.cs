#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Repository
{
	public interface IRepositoryHelper
	{
		Task<IReadOnlyCollection<T>> ToReadOnlyCollectionAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken);

		Task<IReadOnlyDictionary<TKey, TValue>> ToReadOnlyDictionaryAsync<T, TKey, TValue>(IQueryable<T> queryable, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, CancellationToken cancellationToken);

		Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

		Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken);

		Task<T> FirstAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

		Task<T> FirstAsync<T>(IQueryable<T> source, CancellationToken cancellationToken);

		Task<int> CountAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

		Task<int> CountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken);

		Task<bool> AnyAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

		Task<bool> AnyAsync<T>(IQueryable<T> source, CancellationToken cancellationToken);

		IQueryable<T> Include<T, TProperty>(IQueryable<T> source, Expression<Func<T, TProperty>> path);

		Task<int> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken);

		Task<int?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken);

		Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken);

		Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken);

		Task<long> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken);

		Task<long?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken);

		Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken);

		Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken);

		Task<float> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken);

		Task<float?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken);

		Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken);

		Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken);

		Task<double> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken);

		Task<double?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken);

		Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken);

		Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken);

		IQueryable<TSource> AsExpandable<TSource>(IQueryable<TSource> source);
	}
}