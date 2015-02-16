#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Repository
{
	/// <summary>
	/// Представляет SQL-репозиторий.
	/// </summary>
	public interface ISqlRepository : IDisposable
	{
		/// <summary>
		/// Выполняет SQL-скрипт <paramref name="sql" /> с параметрами <paramref name="parameters" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности-результата запроса.</typeparam>
		/// <param name="sql">SQL-скрипт.</param>
		/// <param name="parameters">Параметры SQL-скрипта.</param>
		/// <returns>Результат запроса.</returns>
		IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params KeyValuePair<string, object>[] parameters);

		Task<int> ExecuteSqlCommandAsync(string sql, params KeyValuePair<string, object>[] parameters);
	}
}