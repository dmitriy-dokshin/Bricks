#region

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace Bricks.DAL.Repository
{
	/// <summary>
	/// Интерфейс общего репозитория.
	/// </summary>
	/// <remarks>
	/// Упрощенная версия, позже интегрировать с миркатом. Использование версии мирката усложняется зависимостями.
	/// </remarks>
	public interface IRepository
	{
		/// <summary>
		/// Возвращает запрос для сущности типа <typeparamref name="TEntity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <returns>Запрос для сущности типа <typeparamref name="TEntity" />.</returns>
		IQueryable<TEntity> Select<TEntity>() where TEntity : class;

		/// <summary>
		/// Добавляет сущности <paramref name="entities" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entities">Сущности, которые нужно добавить.</param>
		/// <returns>Задача, результатом которой являются добавленные сущности.</returns>
		Task<IEnumerable<TEntity>> InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

		/// <summary>
		/// Добавляет сущность <paramref name="entity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно добавить.</param>
		/// <returns>Задача, результатом которой является добавленная сущность.</returns>
		Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Обновляет сущности <paramref name="entities" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entities">Сущности, которые нужно обновить.</param>
		/// <returns>Задача, результатом которой являются обновленные сущности.</returns>
		Task<IEnumerable<TEntity>> UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

		/// <summary>
		/// Обновляет сущность <paramref name="entity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно обновить.</param>
		/// <returns>Задача, результатом которой являются обновленная сущность.</returns>
		Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Удаляет сущности <paramref name="entities" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entities">Сущности, которые нужно удалить.</param>
		/// <returns>Задача удаления сущностей.</returns>
		Task DeleteRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

		/// <summary>
		/// Удаляет сущность <paramref name="entity" />.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно удалить.</param>
		/// <returns>Задача удаления сущности.</returns>
		Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Перезагружает сущность.
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности.</typeparam>
		/// <param name="entity">Сущность, которую нужно перезагрузить.</param>
		/// <returns />
		Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Получает транзакцию.
		/// </summary>
		/// <param name="isolationLevel">Уровень блокировки транзакции.</param>
		/// <returns>Объект транзакции.</returns>
		ITransactionScope GetTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
	}
}