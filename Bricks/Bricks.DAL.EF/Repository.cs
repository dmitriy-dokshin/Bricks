#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Collections;
using Bricks.Core.Exceptions;
using Bricks.Core.Extensions;
using Bricks.Core.Repository;
using Bricks.Core.Results;
using Bricks.Core.Tasks;

#endregion

namespace Bricks.DAL.EF
{
	internal sealed class Repository : IRepository, ISqlRepository
	{
		private static readonly string _traceCategory = typeof(Repository).Name;
		private readonly CancellationToken _cancellationToken;
		private readonly ICollectionHelper _collectionHelper;
		private readonly DbContext _dbContext;
		private readonly IExceptionHelper _exceptionHelper;
		private readonly TraceSwitch _traceSwitch = new TraceSwitch(_traceCategory, Resources.Repository_TraceSwitch_Description);

		public Repository(DbContext dbContext, ICancellationTokenProvider cancellationTokenProvider, IExceptionHelper exceptionHelper, ICollectionHelper collectionHelper)
		{
			_dbContext = dbContext;
			_exceptionHelper = exceptionHelper;
			_collectionHelper = collectionHelper;
			_cancellationToken = cancellationTokenProvider.GetCancellationToken();
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_dbContext.Dispose();
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

		#region Implementation of IRepository

		public IQueryable<TEntity> Select<TEntity>() where TEntity : class
		{
			return _dbContext.Set<TEntity>();
		}

		public TEnumerable AddRange<TEntity, TEnumerable>(TEnumerable entities) where TEntity : class where TEnumerable : IEnumerable<TEntity>
		{
			var dbSet = _dbContext.Set<TEntity>();
			dbSet.AddRange(entities);
			return entities;
		}

		public TEntity Add<TEntity>(TEntity entity) where TEntity : class
		{
			var enumerable = _collectionHelper.Single(entity);
			return AddRange<TEntity, IEnumerable<TEntity>>(enumerable).First();
		}

		public TEnumerable UpdateRange<TEntity, TEnumerable>(TEnumerable entities) where TEntity : class where TEnumerable : IEnumerable<TEntity>
		{
			var dbSet = _dbContext.Set<TEntity>();
			foreach (var entity in entities)
			{
				var dbEntityEntry = _dbContext.Entry(entity);
				if (dbEntityEntry.State == EntityState.Detached)
				{
					dbSet.Attach(entity);
					dbEntityEntry = _dbContext.Entry(entity);
				}

				dbEntityEntry.State = EntityState.Modified;
			}

			return entities;
		}

		public TEntity Update<TEntity>(TEntity entity) where TEntity : class
		{
			var enumerable = _collectionHelper.Single(entity);
			return UpdateRange<TEntity, IEnumerable<TEntity>>(enumerable).First();
		}

		public TEnumerable AddOrUpdateRange<TEntity, TEnumerable>(TEnumerable entities) where TEntity : class where TEnumerable : IEnumerable<TEntity>
		{
			var dbSet = _dbContext.Set<TEntity>();
			dbSet.AddOrUpdate(entities.ToArrayIfNot());
			return entities;
		}

		public TEntity AddOrUpdate<TEntity>(TEntity entity) where TEntity : class
		{
			var enumerable = _collectionHelper.Single(entity);
			return AddOrUpdateRange<TEntity, IEnumerable<TEntity>>(enumerable).First();
		}

		public void RemoveRange<TEntity, TEnumerable>(TEnumerable entities) where TEntity : class where TEnumerable : IEnumerable<TEntity>
		{
			var dbSet = _dbContext.Set<TEntity>();
			foreach (var entity in entities)
			{
				var dbEntityEntry = _dbContext.Entry(entity);
				if (dbEntityEntry.State == EntityState.Detached)
				{
					dbSet.Attach(entity);
					dbEntityEntry = _dbContext.Entry(entity);
				}

				dbEntityEntry.State = EntityState.Deleted;
			}
		}

		public void Remove<TEntity>(TEntity entity) where TEntity : class
		{
			var enumerable = _collectionHelper.Single(entity);
			RemoveRange<TEntity, IEnumerable<TEntity>>(enumerable);
		}

		public void Reload<TEntity>(TEntity entity) where TEntity : class
		{
			var dbEntityEntry = _dbContext.Entry(entity);
			if (dbEntityEntry.State == EntityState.Detached)
			{
				var dbSet = _dbContext.Set<TEntity>();
				dbSet.Attach(entity);
			}

			_dbContext.Entry(entity).Reload();
		}

		public Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class
		{
			var dbEntityEntry = _dbContext.Entry(entity);
			if (dbEntityEntry.State == EntityState.Detached)
			{
				var dbSet = _dbContext.Set<TEntity>();
				dbSet.Attach(entity);
			}

			return _dbContext.Entry(entity).ReloadAsync(_cancellationToken);
		}

		public ITransactionScope GetTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			var dbContextTransaction = _dbContext.Database.BeginTransaction(isolationLevel);
			return new TransactionScope(dbContextTransaction);
		}

		public async Task<IResult> SaveAsync()
		{
			IResult result =
				await _exceptionHelper.CatchAsync(
					() => (Task)_dbContext.SaveChangesAsync(_cancellationToken),
					Resources.Repository_Save_ExceptionMessage,
					typeof(DbEntityValidationException),
					typeof(DbUpdateException),
					typeof(DBConcurrencyException));
			return TraceIfNeeded(result);
		}

		public IResult Save()
		{
			IResult<int> result =
				_exceptionHelper.Catch(
					() => _dbContext.SaveChanges(),
					Resources.Repository_Save_ExceptionMessage,
					typeof(DbEntityValidationException),
					typeof(DbUpdateException),
					typeof(DBConcurrencyException));
			return TraceIfNeeded(result);
		}

		private IResult TraceIfNeeded<TResult>(TResult result)
			where TResult : IResult
		{
			if (!result.Success && _traceSwitch.TraceVerbose)
			{
				IResult innerResult = result.GetInnerResult();
				if (innerResult.Exception != null)
				{
					StringBuilder messageBuilder = new StringBuilder();
					IEnumerable<Exception> exceptionHierarchy = innerResult.Exception.GetExceptionHierarchy();
					foreach (var summary in exceptionHierarchy.Select(x => _exceptionHelper.GetSummary(x)))
					{
						messageBuilder.AppendLine(summary);
					}

					string message = messageBuilder.ToString();
					Trace.WriteLine(message, _traceCategory);
				}
			}

			return result;
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
			var sqlParameters = GetParameters(parameters);
			// ReSharper disable CoVariantArrayConversion
			return _dbContext.Database.SqlQuery<TEntity>(sql, sqlParameters);
			// ReSharper restore CoVariantArrayConversion
		}

		public Task<int> ExecuteSqlCommandAsync(string sql, params KeyValuePair<string, object>[] parameters)
		{
			return _dbContext.Database.ExecuteSqlCommandAsync(sql, _cancellationToken, parameters);
		}

		#endregion
	}
}