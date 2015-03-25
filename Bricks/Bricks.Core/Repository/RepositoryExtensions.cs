#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Results;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Repository
{
	public static class RepositoryExtensions
	{
		private static readonly Lazy<IRepositoryHelper> _repositoryHelper;
		private static readonly Lazy<IResultFactory> _resultFactory;

		static RepositoryExtensions()
		{
			_repositoryHelper = new Lazy<IRepositoryHelper>(
				ServiceLocator.Current.GetInstance<IRepositoryHelper>, true);
			_resultFactory = new Lazy<IResultFactory>(
				ServiceLocator.Current.GetInstance<IResultFactory>, true);
		}

		public static IEnumerable<TEntity> AddRange<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			return repository.AddRange<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static IEnumerable<TEntity> UpdateRange<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			return repository.UpdateRange<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static IEnumerable<TEntity> AddOrUpdateRange<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			return repository.AddOrUpdateRange<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static void RemoveRange<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			repository.RemoveRange<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static IReadOnlyCollection<TEntity> AddRange<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			return repository.AddRange<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static IReadOnlyCollection<TEntity> UpdateRange<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			return repository.UpdateRange<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static IReadOnlyCollection<TEntity> AddOrUpdateRange<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			return repository.AddOrUpdateRange<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static void RemoveRange<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			repository.RemoveRange<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static Task<IResult<TEnumerable>> AddRangeAndSaveAsync<TEntity, TEnumerable>(this IRepository repository, TEnumerable entities)
			where TEntity : class
			where TEnumerable : IEnumerable<TEntity>
		{
			return repository.ChangeAndSaveAsync(x => x.AddRange<TEntity, TEnumerable>(entities));
		}

		public static Task<IResult<IEnumerable<TEntity>>> AddRangeAndSaveAsync<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			return repository.AddRangeAndSaveAsync<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static Task<IResult<IReadOnlyCollection<TEntity>>> AddRangeAndSaveAsync<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			return repository.AddRangeAndSaveAsync<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static Task<IResult<TEntity>> AddAndSaveAsync<TEntity>(this IRepository repository, TEntity entity) where TEntity : class
		{
			return repository.ChangeAndSaveAsync(x => x.Add(entity));
		}

		public static Task<IResult<TEnumerable>> UpdateRangeAndSaveAsync<TEntity, TEnumerable>(this IRepository repository, TEnumerable entities)
			where TEntity : class
			where TEnumerable : IEnumerable<TEntity>
		{
			return repository.ChangeAndSaveAsync(x => x.UpdateRange<TEntity, TEnumerable>(entities));
		}

		public static Task<IResult<IEnumerable<TEntity>>> UpdateRangeAndSaveAsync<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			return repository.UpdateRangeAndSaveAsync<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static Task<IResult<IReadOnlyCollection<TEntity>>> UpdateRangeAndSaveAsync<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			return repository.UpdateRangeAndSaveAsync<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static Task<IResult<TEntity>> UpdateAndSaveAsync<TEntity>(this IRepository repository, TEntity entity) where TEntity : class
		{
			return repository.ChangeAndSaveAsync(x => x.Update(entity));
		}

		public static Task<IResult<TEnumerable>> AddOrUpdateRangeAndSaveAsync<TEntity, TEnumerable>(this IRepository repository, TEnumerable entities)
			where TEntity : class
			where TEnumerable : IEnumerable<TEntity>
		{
			return repository.ChangeAndSaveAsync(x => x.AddOrUpdateRange<TEntity, TEnumerable>(entities));
		}

		public static Task<IResult<IEnumerable<TEntity>>> AddOrUpdateRangeAndSaveAsync<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			return repository.AddOrUpdateRangeAndSaveAsync<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static Task<IResult<IReadOnlyCollection<TEntity>>> AddOrUpdateRangeAndSaveAsync<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			return repository.AddOrUpdateRangeAndSaveAsync<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static Task<IResult<TEntity>> AddOrUpdateAndSaveAsync<TEntity>(this IRepository repository, TEntity entity) where TEntity : class
		{
			return repository.ChangeAndSaveAsync(x => x.AddOrUpdate(entity));
		}

		public static Task<IResult> RemoveRangeAndSaveAsync<TEntity, TEnumerable>(this IRepository repository, TEnumerable entities)
			where TEntity : class
			where TEnumerable : IEnumerable<TEntity>
		{
			return repository.ChangeAndSaveAsync(x => x.RemoveRange<TEntity, TEnumerable>(entities));
		}

		public static Task<IResult> RemoveRangeAndSaveAsync<TEntity>(this IRepository repository, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			return repository.RemoveRangeAndSaveAsync<TEntity, IEnumerable<TEntity>>(entities);
		}

		public static Task<IResult> RemoveRangeAndSaveAsync<TEntity>(this IRepository repository, IReadOnlyCollection<TEntity> entities)
			where TEntity : class
		{
			return repository.RemoveRangeAndSaveAsync<TEntity, IReadOnlyCollection<TEntity>>(entities);
		}

		public static Task<IResult> RemoveAndSaveAsync<TEntity>(this IRepository repository, TEntity entity) where TEntity : class
		{
			return repository.ChangeAndSaveAsync(x => x.Remove(entity));
		}

		public static Task<IReadOnlyCollection<T>> ToReadOnlyCollectionAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.ToReadOnlyCollectionAsync(queryable, cancellationToken);
		}

		public static Task<IReadOnlyList<T>> ToReadOnlyListAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.ToReadOnlyListAsync(queryable, cancellationToken);
		}

		public static Task<IReadOnlyDictionary<TKey, TValue>> ToReadOnlyDictionaryAsync<T, TKey, TValue>(this IQueryable<T> queryable, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.ToReadOnlyDictionaryAsync(queryable, keySelector, valueSelector, cancellationToken);
		}

		public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.FirstOrDefaultAsync(queryable, predicate, cancellationToken);
		}

		public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.FirstOrDefaultAsync(queryable, cancellationToken);
		}

		public static Task<T> FirstAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.FirstAsync(queryable, predicate, cancellationToken);
		}

		public static Task<T> FirstAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.FirstAsync(queryable, cancellationToken);
		}

		public static Task<int> CountAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.CountAsync(source, predicate, cancellationToken);
		}

		public static Task<int> CountAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.CountAsync(source, cancellationToken);
		}

		public static Task<bool> AnyAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.AnyAsync(source, predicate, cancellationToken);
		}

		public static Task<bool> AnyAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.AnyAsync(source, cancellationToken);
		}

		public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> queryable, Expression<Func<T, TProperty>> path)
		{
			return _repositoryHelper.Value.Include(queryable, path);
		}

		public static Task<int> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<int?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<int> SumAsync(this IQueryable<int> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static Task<int?> SumAsync(this IQueryable<int?> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static Task<long> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<long?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<long> SumAsync(this IQueryable<long> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static Task<long?> SumAsync(this IQueryable<long?> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static Task<float> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<float?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<float> SumAsync(this IQueryable<float> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static Task<float?> SumAsync(this IQueryable<float?> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static Task<double> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<double?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, selector, cancellationToken);
		}

		public static Task<double> SumAsync(this IQueryable<double> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static Task<double?> SumAsync(this IQueryable<double?> source, CancellationToken cancellationToken)
		{
			return _repositoryHelper.Value.SumAsync(source, cancellationToken);
		}

		public static async Task<IResult<TData>> ChangeAndSaveAsync<TData>(this IRepository repository, Func<IRepository, TData> change)
		{
			TData data = change(repository);
			IResult saveResult = await repository.SaveAsync();
			IResult<TData> result =
				saveResult.Success
					? _resultFactory.Value.Create(data)
					: _resultFactory.Value.CreateUnsuccessfulResult<TData>(innerResult: saveResult);
			return result;
		}

		public static async Task<IResult> ChangeAndSaveAsync(this IRepository repository, Action<IRepository> change)
		{
			change(repository);
			IResult saveResult = await repository.SaveAsync();
			IResult result =
				saveResult.Success
					? _resultFactory.Value.Create()
					: _resultFactory.Value.CreateUnsuccessfulResult(innerResult: saveResult);
			return result;
		}

		public static IQueryable<TSource> AsExpandable<TSource>(this IQueryable<TSource> source)
		{
			return _repositoryHelper.Value.AsExpandable(source);
		}
	}
}