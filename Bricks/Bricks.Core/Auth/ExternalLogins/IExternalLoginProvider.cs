#region

using System;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Auth.ExternalLogins
{
	public interface IExternalLoginProvider
	{
		Uri GetAuthorizeUrl(string scope, string redirectUrl);

		Task<IResult<IAccessTokenData>> GetAccessToken(string code, string redirectUrl);

		Task<IResult<IExternalLoginData>> GetExternalLoginData(string accessToken);
	}
}