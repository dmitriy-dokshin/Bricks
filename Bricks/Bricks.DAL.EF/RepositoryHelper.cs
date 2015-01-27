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

		#endregion
	}
}