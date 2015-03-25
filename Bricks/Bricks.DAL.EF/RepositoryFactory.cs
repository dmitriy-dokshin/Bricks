#region

using System;
using System.Data.Entity;

using Bricks.Core.Repository;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.DAL.EF
{
	internal sealed class RepositoryFactory : IRepositoryFactory
	{
		private readonly IUnityContainer _unityContainer;

		public RepositoryFactory(IUnityContainer unityContainer)
		{
			_unityContainer = unityContainer;
		}

		#region Implementation of IRepositoryFactory

		public IRepository GetRepository(string name, TimeSpan? timeout = null, bool autoDetectChangesEnabled = true, bool validateOnSaveEnabled = true)
		{
			DbContext dbContext = GetDbContext(name, timeout, autoDetectChangesEnabled, validateOnSaveEnabled);
			var repository = _unityContainer.Resolve<IRepository>(new DependencyOverride(typeof(DbContext), dbContext));
			return repository;
		}

		public ISqlRepository GetSqlRepository(string name, TimeSpan? timeout = null)
		{
			DbContext dbContext = GetDbContext(name, timeout, false, false);
			var repository = _unityContainer.Resolve<ISqlRepository>(new DependencyOverride(typeof(DbContext), dbContext));
			return repository;
		}

		private DbContext GetDbContext(string name, TimeSpan? timeout, bool autoDetectChangesEnabled, bool validateOnSaveEnabled)
		{
			var dbContext = _unityContainer.Resolve<DbContext>(name);
			if (timeout.HasValue)
			{
				dbContext.Database.CommandTimeout = Convert.ToInt32(timeout.Value.TotalSeconds);
			}

			dbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
			dbContext.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
			return dbContext;
		}

		#endregion
	}
}