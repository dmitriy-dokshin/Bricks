#region

using System;
using System.Security.Claims;
using Bricks.OWIN.Auth;
using Microsoft.Owin.Security;

#endregion

namespace Bricks.OWIN.Impl.Auth
{
	internal sealed class AccessTokenProvider : IAccessTokenProvider
	{
		private readonly ISecureDataFormat<AuthenticationTicket> _accessTokenFormat;

		public AccessTokenProvider(ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
		{
			_accessTokenFormat = accessTokenFormat;
		}

		#region Implementation of IAccessTokenProvider

		public string CreateAccessToken(ClaimsIdentity claimsIdentity, TimeSpan lifetime)
		{
			var authenticationProperties = new AuthenticationProperties();
			DateTimeOffset now = DateTimeOffset.Now;
			authenticationProperties.IssuedUtc = now;
			authenticationProperties.ExpiresUtc = now.Add(lifetime);
			var ticket = new AuthenticationTicket(claimsIdentity, authenticationProperties);
			string accesstoken = _accessTokenFormat.Protect(ticket);
			return accesstoken;
		}

		#endregion
	}
}