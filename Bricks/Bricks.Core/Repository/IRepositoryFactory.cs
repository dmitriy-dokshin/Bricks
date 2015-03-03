#region

using System;

#endregion

namespace Bricks.Core.Repository
{
	public interface IRepositoryFactory
	{
		IRepository GetRepository(string name, TimeSpan? timeout = null);

		ISqlRepository GetSqlRepository(string name, TimeSpan? timeout = null);
	}
}