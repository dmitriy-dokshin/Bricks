#region

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bricks.DAL.Repository;

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
			return source.FirstOrDefaultAsync(predicate, cancellationToken);
		}

		public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return source.FirstOrDefaultAsync(cancellationToken);
		}

		public Task<T> FirstAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return source.FirstAsync(predicate, cancellationToken);
		}

		public Task<T> FirstAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return source.FirstAsync(cancellationToken);
		}

		public Task<int> CountAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return source.CountAsync(predicate, cancellationToken);
		}

		public Task<int> CountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return source.CountAsync(cancellationToken);
		}

		public Task<bool> AnyAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return source.AnyAsync(predicate, cancellationToken);
		}

		public Task<bool> AnyAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
		{
			return source.AnyAsync(cancellationToken);
		}

		public IQueryable<T> Include<T, TProperty>(IQueryable<T> source, Expression<Func<T, TProperty>> path)
		{
			return source.Include(path);
		}

		#endregion
	}
}