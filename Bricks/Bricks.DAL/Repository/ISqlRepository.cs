#region

using System.Collections.Generic;

#endregion

namespace Bricks.DAL.Repository
{
	/// <summary>
	/// Представляет SQL-репозиторий.
	/// </summary>
	public interface ISqlRepository
	{
		/// <summary>
		/// Выполняет SQL-скрипт <paramref name="sql" /> с параметрами <paramref name="parameters" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности-результата запроса.</typeparam>
		/// <param name="sql">SQL-скрипт.</param>
		/// <param name="parameters">Параметры SQL-скрипта.</param>
		/// <returns>Результат запроса.</returns>
		IEnumerable<TEntity> ExecuteSql<TEntity>(string sql, params KeyValuePair<string, object>[] parameters);
	}
}