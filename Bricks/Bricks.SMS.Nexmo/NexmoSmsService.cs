#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bricks.Core.IoC;
using Bricks.Web;

using Microsoft.Practices.ServiceLocation;

using Newtonsoft.Json.Linq;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Реализя <see cref="ISmsService" /> для отправки сообщений через API Nexmo.
	/// </summary>
	public sealed class NexmoSmsService : ISmsService
	{
		private readonly IServiceLocator _serviceLocator;
		private readonly IWebHelper _webHelper;

		public NexmoSmsService(IServiceLocator serviceLocator, IWebHelper webHelper)
		{
			_serviceLocator = serviceLocator;
			_webHelper = webHelper;
		}

		#region Implementation of ISmsService

		/// <summary>
		/// Отправляет SMS с текстом <paramref name="text" /> на номер телефона <paramref name="phoneNumber" />.
		/// </summary>
		/// <param name="phoneNumber">Номер телефона.</param>
		/// <param name="text">Текст сообщения.</param>
		/// <returns />
		public async Task SendAsync(string phoneNumber, string text)
		{
			var smsParameters = new SmsParameters(phoneNumber, text);
			_serviceLocator.BuildUp(smsParameters);

			Tuple<SmsResult, JObject> tuple = await _webHelper.Execute<SmsParameters, SmsResult, JObject>(
																										  smsParameters.ServiceUrl, HttpMethod.Post, smsParameters, ContentType.Json);
			SmsResult smsResult = tuple.Item1;
			if (smsResult == null)
			{
				// todo: Логировать.
			}
			else
			{
				IEnumerable<MessagePartDto> badMessages =
				smsResult.Messages.Where(x => x.Status != NexmoResponseCode.Success);
				foreach (MessagePartDto badMessage in badMessages)
				{
					// todo: Логировать.
				}
			}
		}

		#endregion
	}
}