#region

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

#endregion

namespace Bricks.EF.Extensions
{
	public static class DbContextExtensions
	{
		public static DbContext Add<T>(this DbContext dbContext, T entity) where T : class
		{
			dbContext.Set<T>().Add(entity);
			return dbContext;
		}

		public static DbContext AddRange<T>(this DbContext dbContext, IEnumerable<T> entities) where T : class
		{
			dbContext.Set<T>().AddRange(entities);
			return dbContext;
		}

		public static DbContext AddOrUpdate<T>(this DbContext dbContext, T entity) where T : class
		{
			dbContext.Set<T>().AddOrUpdate(entity);
			return dbContext;
		}

		public static DbContext AddOrUpdateRange<T>(this DbContext dbContext, IEnumerable<T> entities) where T : class
		{
			foreach (var entity in entities)
			{
				dbContext.AddOrUpdate(entity);
			}

			return dbContext;
		}

		public static DbContext Update<T>(this DbContext dbContext, T entity) where T : class
		{
			var dbEntityEntry = dbContext.Entry(entity);
			var dbSet = dbContext.Set<T>();
			if (dbEntityEntry.State == EntityState.Detached)
			{
				dbSet.Attach(entity);
				dbEntityEntry = dbContext.Entry(entity);
			}

			dbEntityEntry.State = EntityState.Modified;
			return dbContext;
		}

		public static DbContext UpdateRange<T>(this DbContext dbContext, IEnumerable<T> entities) where T : class
		{
			var entitiesArray = entities as T[] ?? entities.ToArray();
			foreach (var entity in entitiesArray)
			{
				Update(dbContext, entity);
			}

			return dbContext;
		}

		public static DbContext Remove<T>(this DbContext dbContext, T entity) where T : class
		{
			var dbEntityEntry = dbContext.Entry(entity);
			var dbSet = dbContext.Set<T>();
			if (dbEntityEntry.State == EntityState.Detached)
			{
				dbSet.Attach(entity);
				dbEntityEntry = dbContext.Entry(entity);
			}

			dbEntityEntry.State = EntityState.Deleted;
			return dbContext;
		}

		public static DbContext RemoveRange<T>(this DbContext dbContext, IEnumerable<T> entities) where T : class
		{
			foreach (var entity in entities)
			{
				Remove(dbContext, entity);
			}

			return dbContext;
		}

		public static IEnumerable<IQueryable<T>> Sets<T>(this IEnumerable<DbContext> dbContexts) where T : class
		{
			return dbContexts.Select(x => x.Set<T>());
		}
	}
}