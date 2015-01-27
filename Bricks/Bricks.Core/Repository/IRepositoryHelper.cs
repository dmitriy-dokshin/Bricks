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
	}
}