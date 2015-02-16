namespace Bricks.Core.Repository
{
	public interface IRepositoryFactory
	{
		IRepository GetRepository(string name);

		ISqlRepository GetSqlRepository(string name);
	}
}