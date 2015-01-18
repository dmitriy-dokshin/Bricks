#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bricks.Core.Auth.ExternalLogins;
using Bricks.Core.Configuration;
using Bricks.Core.Results;
using Bricks.Core.Web;

using Newtonsoft.Json.Linq;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	internal sealed class VkLoginProvider : IExternalLoginProvider
	{
		private const string VkSettingsKey = "vkSettings";
		private static readonly Uri _usersGetUrl = new Uri("https://api.vk.com/method/users.get");
		private static readonly Uri _accessTokenUrl = new Uri("https://oauth.vk.com/access_token");
		private static readonly Uri _authorizeUrl = new Uri("https://oauth.vk.com/authorize");
		private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);
		private readonly IResultFactory _resultFactory;
		private readonly IUrlHelper _urlHelper;
		private readonly IVkSettings _vkSettings;
		private readonly IWebHelper _webHelper;

		public VkLoginProvider(IResultFactory resultFactory, IWebHelper webHelper, IConfigurationManager configurationManager, IUrlHelper urlHelper)
		{
			_resultFactory = resultFactory;
			_webHelper = webHelper;
			_urlHelper = urlHelper;
			_vkSettings = configurationManager.GetSettings<IVkSettings>(VkSettingsKey);
		}

		#region Implementation of IExternalLoginProvider

		public Uri GetAuthorizeUrl(string scope, string redirectUrl)
		{
			var vkAuthorizeParameters = new VkAuthorizeParameters(_vkSettings.ClientId, scope, redirectUrl);
			return _urlHelper.GetUrl(_authorizeUrl, vkAuthorizeParameters);
		}

		public async Task<IResult<IAccessTokenData>> GetAccessToken(string code, string redirectUrl)
		{
			var vkAccessTokenParameters = new VkAccessTokenParameters(_vkSettings.ClientId, _vkSettings.ClientSecret, code, redirectUrl);
			Tuple<VkAccessTokenData, JObject> accessTokenResult =
				await _webHelper.Execute<VkAccessTokenParameters, VkAccessTokenData, JObject>(
					_accessTokenUrl, HttpMethod.Get, vkAccessTokenParameters, ContentType.Json, _timeout);
			if (accessTokenResult.Item2 != null)
			{
				var message = accessTokenResult.Item2["error_description"].Value<string>();
				return _resultFactory.CreateUnsuccessfulResult<IAccessTokenData>(message);
			}

			return _resultFactory.Create(accessTokenResult.Item1);
		}

		public async Task<IResult<IExternalLoginData>> GetExternalLoginData(string accessToken)
		{
			Tuple<VkResponseData<IReadOnlyCollection<VkUserData>>, JObject> usersGetResult =
				await _webHelper.Execute<VkUserParameters, VkResponseData<IReadOnlyCollection<VkUserData>>, JObject>(
					_usersGetUrl, HttpMethod.Get, new VkUserParameters(accessToken), ContentType.Json, _timeout);
			if (usersGetResult.Item2 != null)
			{
				var message = usersGetResult.Item2["error"]["error_msg"].Value<string>();
				return _resultFactory.CreateUnsuccessfulResult<IExternalLoginData>(message);
			}

			return _resultFactory.Create(usersGetResult.Item1.Response.FirstOrDefault());
		}

		#endregion
	}
}