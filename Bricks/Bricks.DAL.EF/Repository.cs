#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Tasks;
using Bricks.DAL.Repository;

#endregion

namespace Bricks.DAL.EF
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IRepository" />.
	/// </summary>
	internal sealed class Repository : IRepository, ISqlRepository
	{
		private readonly CancellationToken _cancellationToken;
		private readonly DbContext _dbContext;

		public Repository(DbContext dbContext, ICancellationTokenProvider cancellationTokenProvider)
		{
			_dbContext = dbContext;
			_cancellationToken = cancellationTokenProvider.GetCancellationToken();
		}

		#region Implementation of IRepository

		/// <summary>
		/// Возвращает запрос для сущности типа <typeparamref name="TEntity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <returns>Запрос для сущности типа <typeparamref name="TEntity" />.</returns>
		public IQueryable<TEntity> Select<TEntity>() where TEntity : class
		{
			return _dbContext.Set<TEntity>();
		}

		/// <summary>
		/// Добавляет сущности <paramref name="entities" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entities">Сущности, которые нужно добавить.</param>
		/// <returns>Задача, результатом которой являются добавленные сущности.</returns>
		public async Task<IEnumerable<TEntity>> InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
		{
			_dbContext.Set<TEntity>().AddRange(entities);
			await _dbContext.SaveChangesAsync(_cancellationToken);
			return entities;
		}

		/// <summary>
		/// Добавляет сущность <paramref name="entity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно добавить.</param>
		/// <returns>Задача, результатом которой является добавленная сущность.</returns>
		public async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class
		{
			_dbContext.Set<TEntity>().Add(entity);
			await _dbContext.SaveChangesAsync(_cancellationToken);
			return entity;
		}

		/// <summary>
		/// Обновляет сущности <paramref name="entities" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entities">Сущности, которые нужно обновить.</param>
		/// <returns>Задача, результатом которой являются обновленные сущности.</returns>
		public async Task<IEnumerable<TEntity>> UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
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

			await _dbContext.SaveChangesAsync(_cancellationToken);
			return entities;
		}

		/// <summary>
		/// Обновляет сущность <paramref name="entity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно обновить.</param>
		/// <returns>Задача, результатом которой являются обновленная сущность.</returns>
		public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
		{
			await _dbContext.SaveChangesAsync(_cancellationToken);
			return entity;
		}

		/// <summary>
		/// Удаляет сущности <paramref name="entities" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entities">Сущности, которые нужно удалить.</param>
		/// <returns>Задача удаления сущностей.</returns>
		public Task DeleteRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
		{
			_dbContext.Set<TEntity>().RemoveRange(entities);
			return _dbContext.SaveChangesAsync(_cancellationToken);
		}

		/// <summary>
		/// Удаляет сущность <paramref name="entity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно удалить.</param>
		/// <returns>Задача удаления сущности.</returns>
		public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
		{
			_dbContext.Set<TEntity>().Remove(entity);
			return _dbContext.SaveChangesAsync(_cancellationToken);
		}

		/// <summary>
		/// Перезагружает сущность.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно перезагрузить.</param>
		/// <returns />
		public Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class
		{
			return _dbContext.Entry(entity).ReloadAsync(_cancellationToken);
		}

		/// <summary>
		/// Получает транзакцию.
		/// </summary>
		/// <param name="isolationLevel">Уровень блокировки транзакции.</param>
		/// <returns>Объект транзакции.</returns>
		public ITransactionScope GetTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			DbContextTransaction dbContextTransaction = _dbContext.Database.BeginTransaction(isolationLevel);
			return new TransactionScope(dbContextTransaction);
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
		public IEnumerable<TEntity> ExecuteSql<TEntity>(string sql, params KeyValuePair<string, object>[] parameters)
		{
			SqlParameter[] sqlParameters = parameters.Select(x => new SqlParameter(string.Format(CultureInfo.InvariantCulture, "@{0}", x.Key), x.Value ?? DBNull.Value)).ToArray();
			// ReSharper disable CoVariantArrayConversion
			return _dbContext.Database.SqlQuery<TEntity>(sql, sqlParameters);
			// ReSharper restore CoVariantArrayConversion
		}

		#endregion

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