#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Exceptions;
using Bricks.Core.Results;
using Bricks.Core.Tasks;
using Bricks.DAL.Repository;
using Bricks.Helpers.Collections;

#endregion

namespace Bricks.DAL.EF
{
	internal sealed class Repository : IRepository, ISqlRepository
	{
		private readonly CancellationToken _cancellationToken;
		private readonly ICollectionHelper _collectionHelper;
		private readonly DbContext _dbContext;
		private readonly IExceptionHelper _exceptionHelper;

		public Repository(DbContext dbContext, ICancellationTokenProvider cancellationTokenProvider, IExceptionHelper exceptionHelper, ICollectionHelper collectionHelper)
		{
			_dbContext = dbContext;
			_exceptionHelper = exceptionHelper;
			_collectionHelper = collectionHelper;
			_cancellationToken = cancellationTokenProvider.GetCancellationToken();
		}

		#region Implementation of IRepository

		public IQueryable<TEntity> Select<TEntity>() where TEntity : class
		{
			return _dbContext.Set<TEntity>();
		}

		public TEnumerable AddRange<TEntity, TEnumerable>(TEnumerable entities) where TEntity : class where TEnumerable : IEnumerable<TEntity>
		{
			DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
			dbSet.AddRange(entities);
			return entities;
		}

		public TEntity Add<TEntity>(TEntity entity) where TEntity : class
		{
			IEnumerable<TEntity> enumerable = _collectionHelper.Single(entity);
			return AddRange<TEntity, IEnumerable<TEntity>>(enumerable).First();
		}

		public TEnumerable UpdateRange<TEntity, TEnumerable>(TEnumerable entities) where TEntity : class where TEnumerable : IEnumerable<TEntity>
		{
			DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
			foreach (TEntity entity in entities)
			{
				DbEntityEntry<TEntity> dbEntityEntry = _dbContext.Entry(entity);
				if (dbEntityEntry.State == EntityState.Detached)
				{
					dbSet.Attach(entity);
					dbEntityEntry = _dbContext.Entry(entity);
					dbEntityEntry.State = EntityState.Modified;
				}
			}

			return entities;
		}

		public TEntity Update<TEntity>(TEntity entity) where TEntity : class
		{
			IEnumerable<TEntity> enumerable = _collectionHelper.Single(entity);
			return UpdateRange<TEntity, IEnumerable<TEntity>>(enumerable).First();
		}

		public void RemoveRange<TEntity, TEnumerable>(TEnumerable entities) where TEntity : class where TEnumerable : IEnumerable<TEntity>
		{
			DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
			dbSet.RemoveRange(entities);
		}

		public void Remove<TEntity>(TEntity entity) where TEntity : class
		{
			IEnumerable<TEntity> enumerable = _collectionHelper.Single(entity);
			RemoveRange<TEntity, IEnumerable<TEntity>>(enumerable);
		}

		public Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class
		{
			return _dbContext.Entry(entity).ReloadAsync(_cancellationToken);
		}

		public ITransactionScope GetTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			DbContextTransaction dbContextTransaction = _dbContext.Database.BeginTransaction(isolationLevel);
			return new TransactionScope(dbContextTransaction);
		}

		public Task<IResult> SaveAsync()
		{
			return _exceptionHelper.CatchAsync(() => (Task)_dbContext.SaveChangesAsync(_cancellationToken), Resources.Repository_SaveAsync_ExceptionMessage, typeof(DbEntityValidationException), typeof(DbUpdateException));
		}

		#endregion

		#region Implementation of ISqlRepository

		/// <summary>
		/// Выполняет SQL-скрипт <paramref name="sql" /> с параметрами <paramref name="parameters" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности-результата запроса.</typeparam>
		/// <param name="sql">SQL-скрипт.</param>
		/// <param name="parameters">Параметры SQL-скрипта.</param>
		/// <returns>Результат запроса.</returns>
		public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params KeyValuePair<string, object>[] parameters)
		{
			SqlParameter[] sqlParameters = GetParameters(parameters);
			// ReSharper disable CoVariantArrayConversion
			return _dbContext.Database.SqlQuery<TEntity>(sql, sqlParameters);
			// ReSharper restore CoVariantArrayConversion
		}

		public Task<int> ExecuteSqlCommandAsync(string sql, params KeyValuePair<string, object>[] parameters)
		{
			return _dbContext.Database.ExecuteSqlCommandAsync(sql, _cancellationToken, parameters);
		}

		#endregion

		private static SqlParameter[] GetParameters(KeyValuePair<string, object>[] parameters)
		{
			return parameters.Select(x => new SqlParameter(string.Format(CultureInfo.InvariantCulture, "@{0}", x.Key), x.Value ?? DBNull.Value)).ToArray();
		}

		private sealed class TransactionScope : ITransactionScope
		{
			private readonly DbContextTransaction _dbContextTransaction;

			public TransactionScope(DbContextTransaction dbContextTransaction)
			{
				_dbContextTransaction = dbContextTransaction;
			}

			#region Implementation of IDisposable

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				_dbContextTransaction.Dispose();
			}

			#endregion

			#region Implementation of ITransactionScope

			/// <summary>
			/// Фиксирует транзакцию.
			/// </summary>
			public void Commit()
			{
				_dbContextTransaction.Commit();
			}

			/// <summary>
			/// Откатывает транзакцию.
			/// </summary>
			public void Rollback()
			{
				_dbContextTransaction.Rollback();
			}

			#endregion
		}
	}
}