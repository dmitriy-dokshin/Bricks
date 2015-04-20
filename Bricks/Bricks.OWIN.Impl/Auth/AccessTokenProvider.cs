#region

using System;
using System.Security.Claims;

using Bricks.Core.DateTime;
using Bricks.OWIN.Auth;

using Microsoft.Owin.Security;

#endregion

namespace Bricks.OWIN.Impl.Auth
{
	internal sealed class AccessTokenProvider : IAccessTokenProvider
	{
		private readonly ISecureDataFormat<AuthenticationTicket> _accessTokenFormat;
		private readonly IDateTimeProvider _dateTimeProvider;

		public AccessTokenProvider(ISecureDataFormat<AuthenticationTicket> accessTokenFormat, IDateTimeProvider dateTimeProvider)
		{
			_accessTokenFormat = accessTokenFormat;
			_dateTimeProvider = dateTimeProvider;
		}

		#region Implementation of IAccessTokenProvider

		public string CreateAccessToken(ClaimsIdentity claimsIdentity, TimeSpan lifetime)
		{
			var authenticationProperties = new AuthenticationProperties();
			DateTimeOffset now = _dateTimeProvider.Now;
			authenticationProperties.IssuedUtc = now;
			authenticationProperties.ExpiresUtc = now.Add(lifetime);
			var ticket = new AuthenticationTicket(claimsIdentity, authenticationProperties);
			string accesstoken = _accessTokenFormat.Protect(ticket);
			return accesstoken;
		}

		#endregion
	}
}