﻿#region

using System;
using System.Threading.Tasks;

using Bricks.Core.Auth.ExternalLogins;
using Bricks.Core.Results;
using Bricks.Core.Web;

using Newtonsoft.Json.Linq;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	internal sealed class FacebookLoginProvider : IExternalLoginProvider
	{
		private static readonly Uri _meUrl = new Uri("https://graph.facebook.com/v2.2/me");
		private static readonly Uri _mePictureUrl = new Uri("https://graph.facebook.com/v2.2/me/picture");
		private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);
		private readonly IResultFactory _resultFactory;
		private readonly IWebHelper _webHelper;

		public FacebookLoginProvider(IResultFactory resultFactory, IWebHelper webHelper)
		{
			_resultFactory = resultFactory;
			_webHelper = webHelper;
		}

		#region Implementation of IExternalLoginProvider

		public Uri GetAuthorizeUrl(string scope, string redirectUrl)
		{
			throw new NotImplementedException();
		}

		public Task<IResult<IAccessTokenData>> GetAccessToken(string code, string redirectUrl)
		{
			throw new NotImplementedException();
		}

		public async Task<IResult<IExternalLoginData>> GetExternalLoginData(string accessToken)
		{
			Tuple<FacebookUserData, JObject> meResult =
				await _webHelper.Execute<FacebookUserParameters, FacebookUserData, JObject>(
					_meUrl, HttpMethod.Get, new FacebookUserParameters(accessToken), ContentType.Json, _timeout);
			if (meResult.Item2 != null)
			{
				var message = meResult.Item2["error"]["message"].Value<string>();
				return _resultFactory.CreateUnsuccessfulResult<IExternalLoginData>(message);
			}

			FacebookUserData facebookUserData = meResult.Item1;
			Tuple<FacebookResponseData<FacebookUserPictureData>, JObject> mePictureResult =
				await _webHelper.Execute<FacebookUserPictureParameters, FacebookResponseData<FacebookUserPictureData>, JObject>(
					_mePictureUrl, HttpMethod.Get, new FacebookUserPictureParameters(accessToken), ContentType.Json, _timeout);
			if (mePictureResult.Item2 == null)
			{
				FacebookUserPictureData facebookUserPictureData = mePictureResult.Item1.Data;
				if (!facebookUserPictureData.IsDefault)
				{
					facebookUserData.ImageUrl = facebookUserPictureData.Url;
				}
			}

			return _resultFactory.Create(facebookUserData);
		}

		#endregion
	}
}