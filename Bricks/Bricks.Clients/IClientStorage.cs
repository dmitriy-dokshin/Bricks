namespace Bricks.Clients
{
	/// <summary>
	/// Хранилище подключений клиентов.
	/// </summary>
	/// <typeparam name="TUserId">Тип идентификатор пользователя.</typeparam>
	public interface IClientStorage<TUserId>
	{
		/// <summary>
		/// Получает контекст подключений клиентов.
		/// </summary>
		/// <typeparam name="TClient">Тип клиента.</typeparam>
		/// <param name="key">Ключ контекста подключения клиентов.</param>
		/// <returns>Контекст подключения клиентов.</returns>
		IClientContext<TClient, TUserId> GetClientContext<TClient>(string key) where TClient : class;
	}
}