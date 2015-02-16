#region

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

		public IRepository GetRepository(string name)
		{
			var dbContext = _unityContainer.Resolve<DbContext>(name);
			var repository = _unityContainer.Resolve<IRepository>(new DependencyOverride(typeof(DbContext), dbContext));
			return repository;
		}

		public ISqlRepository GetSqlRepository(string name)
		{
			var dbContext = _unityContainer.Resolve<DbContext>(name);
			var repository = _unityContainer.Resolve<ISqlRepository>(new DependencyOverride(typeof(DbContext), dbContext));
			return repository;
		}

		#endregion
	}
}