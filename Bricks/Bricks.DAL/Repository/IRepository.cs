﻿#region

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.DAL.Repository
{
	public interface IRepository
	{
		IQueryable<TEntity> Select<TEntity>() where TEntity : class;

		TEnumerable AddRange<TEntity, TEnumerable>(TEnumerable entities)
			where TEntity : class
			where TEnumerable : IEnumerable<TEntity>;

		TEntity Add<TEntity>(TEntity entity) where TEntity : class;

		TEnumerable UpdateRange<TEntity, TEnumerable>(TEnumerable entities)
			where TEntity : class
			where TEnumerable : IEnumerable<TEntity>;

		TEntity Update<TEntity>(TEntity entity) where TEntity : class;

		void RemoveRange<TEntity, TEnumerable>(TEnumerable entities)
			where TEntity : class
			where TEnumerable : IEnumerable<TEntity>;

		void Remove<TEntity>(TEntity entity) where TEntity : class;

		Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class;

		ITransactionScope GetTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

		Task<IResult> SaveAsync();
	}
}