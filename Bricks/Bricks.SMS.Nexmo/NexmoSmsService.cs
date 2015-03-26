#region

using System.Linq;
using System.Threading.Tasks;

using Bricks.Core.IoC;
using Bricks.Core.Web;

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

			var sendSmsResult = await _webHelper.Execute<SmsParameters, SmsResult, JObject>(
				smsParameters.ServiceUrl, smsParameters, HttpMethod.Post);
			if (!sendSmsResult.Success)
			{
				// todo: Логировать.
			}
			else
			{
				var badMessages =
					sendSmsResult.Data.Result.Messages.Where(x => x.Status != NexmoResponseCode.Success);
				foreach (var badMessage in badMessages)
				{
					// todo: Логировать.
				}
			}
		}

		#endregion
	}
}