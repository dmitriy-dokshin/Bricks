#region

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Repository;

#endregion

namespace Bricks.DAL.EF
{
	public sealed class RepositoryHelper : IRepositoryHelper
	{
		#region Implementation of IRepositoryHelper

		public async Task<IReadOnlyCollection<T>> ToReadOnlyCollectionAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
		{
			return await queryable.ToArrayAsync(cancellationToken);
		}

		public async Task<IReadOnlyDictionary<TKey, TValue>> ToReadOnlyDictionaryAsync<T, TKey, TValue>(IQueryable<T> queryable, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, CancellationToken cancellationToken)
		{
			return await queryable.ToDictionaryAsync(keySelector, valueSelector, cancellationToken);
		}

		public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return QueryableExtensions.FirstOrDefaultAsync(source, predicate, cancellationToken);
		}

		public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.FirstOrDefaultAsync(source, cancellationToken);
		}

		public Task<T> FirstAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return QueryableExtensions.FirstAsync(source, predicate, cancellationToken);
		}

		public Task<T> FirstAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.FirstAsync(source, cancellationToken);
		}

		public Task<int> CountAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return QueryableExtensions.CountAsync(source, predicate, cancellationToken);
		}

		public Task<int> CountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.CountAsync(source, cancellationToken);
		}

		public Task<bool> AnyAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return QueryableExtensions.AnyAsync(source, predicate, cancellationToken);
		}

		public Task<bool> AnyAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.AnyAsync(source, cancellationToken);
		}

		public IQueryable<T> Include<T, TProperty>(IQueryable<T> source, Expression<Func<T, TProperty>> path)
		{
			return QueryableExtensions.Include(source, path);
		}

		public async Task<int> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
		{
			int sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, selector, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<int?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, selector, cancellationToken);
		}

		public async Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken)
		{
			int sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, cancellationToken);
		}

		public async Task<long> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
		{
			long sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, selector, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<long?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, selector, cancellationToken);
		}

		public async Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken)
		{
			long sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, cancellationToken);
		}

		public async Task<float> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
		{
			float sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, selector, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<float?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, selector, cancellationToken);
		}

		public async Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken)
		{
			float sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, cancellationToken);
		}

		public async Task<double> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
		{
			double sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, selector, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<double?> SumAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, selector, cancellationToken);
		}

		public async Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken)
		{
			double sum;
			try
			{
				sum = await QueryableExtensions.SumAsync(source, cancellationToken);
			}
			catch (InvalidOperationException)
			{
				sum = default(int);
			}

			return sum;
		}

		public Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken)
		{
			return QueryableExtensions.SumAsync(source, cancellationToken);
		}

		#endregion
	}
}