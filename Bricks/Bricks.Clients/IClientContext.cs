#region

using System.Collections.Generic;

#endregion

namespace Bricks.Clients
{
	/// <summary>
	/// Интерфейс контекста подключений клиентов.
	/// </summary>
	/// <typeparam name="TClient">Тип клиента.</typeparam>
	/// <typeparam name="TUserId">Тип идентификатора пользователя.</typeparam>
	public interface IClientContext<out TClient, TUserId>
		where TClient : class
	{
		/// <summary>
		/// Клиент для всех подключений.
		/// </summary>
		TClient All { get; }

		/// <summary>
		/// Возвращает клиента для всех подключений в группе <paramref name="groupName" />.
		/// </summary>
		/// <param name="groupName">Название группы.</param>
		/// <returns>Клиент для всех подключений в группе <paramref name="groupName" />.</returns>
		TClient Group(string groupName);

		/// <summary>
		/// Возвращает клиента для всех подключений в группах <paramref name="groupNames" />.
		/// </summary>
		/// <param name="groupNames">Названия групп.</param>
		/// <returns>Клиент для всех подключений в группах <paramref name="groupNames" />.</returns>
		TClient Groups(IList<string> groupNames);

		/// <summary>
		/// Возвращает клиента для подключений пользователя <paramref name="userId" />.
		/// </summary>
		/// <param name="userId">Идентификатор пользователя.</param>
		/// <returns>Клиент для подключений пользователя <paramref name="userId" />.</returns>
		TClient User(TUserId userId);

		/// <summary>
		/// Возвращает клиента для подключений пользователей <paramref name="userIds" />.
		/// </summary>
		/// <param name="userIds">Идентификаторы пользователей.</param>
		/// <returns>Клиент для подключений пользователей <paramref name="userIds" />.</returns>
		TClient Users(IList<TUserId> userIds);
	}
}