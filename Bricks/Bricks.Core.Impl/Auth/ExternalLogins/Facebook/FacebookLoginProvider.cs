#region

using System;
using System.Threading;
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

		public Task<IResult<IAccessTokenData>> GetAccessToken(string code, string redirectUrl, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<IResult<IExternalLoginData>> GetExternalLoginData(string accessToken, CancellationToken cancellationToken)
		{
			IResult<WebResponseData<FacebookUserData, JObject>> meResult =
				await _webHelper.Execute<FacebookUserParameters, FacebookUserData, JObject>(
					_meUrl, cancellationToken, new FacebookUserParameters(accessToken), timeout: _timeout);
			if (!meResult.Success)
			{
				var message = meResult.Data.ErrorResult["error"]["message"].Value<string>();
				return _resultFactory.CreateUnsuccessfulResult<IExternalLoginData>(message);
			}

			FacebookUserData facebookUserData = meResult.Data.Result;
			IResult<WebResponseData<FacebookResponseData<FacebookUserPictureData>, JObject>> mePictureResult =
				await _webHelper.Execute<FacebookUserPictureParameters, FacebookResponseData<FacebookUserPictureData>, JObject>(
					_mePictureUrl, cancellationToken, new FacebookUserPictureParameters(accessToken), timeout: _timeout);
			if (mePictureResult.Success)
			{
				FacebookUserPictureData facebookUserPictureData = mePictureResult.Data.Result.Data;
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