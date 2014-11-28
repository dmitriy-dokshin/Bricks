#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Bricks.Core.Conversion;

using Microsoft.AspNet.SignalR;

#endregion

namespace Bricks.Clients.SignalR
{
	/// <summary>
	/// Адаптер <see cref="IHubContext{T}" /> для <see cref="IClientContext{TClient,TUserId}" />.
	/// </summary>
	/// <typeparam name="TClient">Тип клиента.</typeparam>
	/// <typeparam name="TUserId">Тип идентификатора пользователя.</typeparam>
	internal sealed class ClientContextAdapter<TClient, TUserId> : IClientContext<TClient, TUserId>
		where TClient : class
	{
		private readonly IHubContext<TClient> _hubContext;

		public ClientContextAdapter(IHubContext<TClient> hubContext)
		{
			_hubContext = hubContext;
		}

		#region Implementation of IClientContext<out TClient>

		/// <summary>
		/// Клиент для всех подключений.
		/// </summary>
		public TClient All
		{
			get
			{
				return _hubContext.Clients.All;
			}
		}

		/// <summary>
		/// Возвращает клиента для всех подключений в группе <paramref name="groupName" />.
		/// </summary>
		/// <param name="groupName">Название группы.</param>
		/// <returns>Клиент для всех подключений в группе <paramref name="groupName" />.</returns>
		public TClient Group(string groupName)
		{
			return _hubContext.Clients.Group(groupName);
		}

		/// <summary>
		/// Возвращает клиента для всех подключений в группах <paramref name="groupNames" />.
		/// </summary>
		/// <param name="groupNames">Названия групп.</param>
		/// <returns>Клиент для всех подключений в группах <paramref name="groupNames" />.</returns>
		public TClient Groups(IList<string> groupNames)
		{
			return _hubContext.Clients.Groups(groupNames);
		}

		/// <summary>
		/// Возвращает клиента для подключений пользователя <paramref name="userId" />.
		/// </summary>
		/// <param name="userId">Идентификатор пользователя.</param>
		/// <returns>Клиент для подключений пользователя <paramref name="userId" />.</returns>
		public TClient User(TUserId userId)
		{
			return _hubContext.Clients.User(GetUserId(userId));
		}

		/// <summary>
		/// Возвращает клиента для подключений пользователей <paramref name="userIds" />.
		/// </summary>
		/// <param name="userIds">Идентификаторы пользователей.</param>
		/// <returns>Клиент для подключений пользователей <paramref name="userIds" />.</returns>
		public TClient Users(IList<TUserId> userIds)
		{
			return _hubContext.Clients.Users(userIds.Select(GetUserId).ToArray());
		}

		#endregion

		private static string GetUserId(TUserId userId)
		{
			var convertible = userId as IConvertible;
			if (convertible != null)
			{
				return convertible.ToString(CultureInfo.InvariantCulture);
			}

			var convertible1 = userId as IConvertible<string>;
			if (convertible1 != null)
			{
				return convertible1.Convert();
			}

			return userId.ToString();
		}
	}
}