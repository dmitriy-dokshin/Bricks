﻿#region

using System.Collections.Immutable;

using Microsoft.AspNet.SignalR;

#endregion

namespace Bricks.Clients.SignalR
{
	/// <summary>
	/// Реализация <see cref="IClientStorage{TUserId}" /> для SignalR.
	/// </summary>
	public class ClientStorage<TUserId> : IClientStorage<TUserId>
	{
		private IImmutableDictionary<string, string> _hubNamesByKey;

		public ClientStorage()
		{
			_hubNamesByKey = ImmutableDictionary<string, string>.Empty;
		}

		#region Implementation of IClientStorage

		/// <summary>
		/// Получает контекст подключений клиентов.
		/// </summary>
		/// <typeparam name="TClient">Тип клиента.</typeparam>
		/// <param name="key">Ключ контекста подключения клиентов.</param>
		/// <returns>Контекст подключения клиентов.</returns>
		public virtual IClientContext<TClient, TUserId> GetClientContext<TClient>(string key) where TClient : class
		{
			var hubName = _hubNamesByKey[key];
			var hubContext = GlobalHost.ConnectionManager.GetHubContext<TClient>(hubName);
			return new ClientContextAdapter<TClient, TUserId>(hubContext);
		}

		#endregion

		public void Register<THub, TClient>(string key) where THub : Hub<TClient> where TClient : class
		{
			var hubName = HubHelper.GetHubName(typeof(THub));
			_hubNamesByKey = _hubNamesByKey.SetItem(key, hubName);
		}
	}
}