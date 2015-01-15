#region

using System.Security.Claims;

#endregion

namespace Bricks.OWIN.Auth
{
	public interface IAccessTokenProvider
	{
		string CreateAccessToken(ClaimsIdentity claimsIdentity);
	}
}