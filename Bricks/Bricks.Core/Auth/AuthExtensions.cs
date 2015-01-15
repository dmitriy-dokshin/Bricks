#region

using System;
using System.Security.Claims;
using System.Security.Principal;

#endregion

namespace Bricks.Core.Auth
{
	public static class AuthExtensions
	{
		public static TUserId GetUserId<TUserId>(this IIdentity identity, Func<string, TUserId> parseFunc)
		{
			if (!identity.IsAuthenticated)
			{
				throw new InvalidOperationException();
			}

			var claimsIdentity = (ClaimsIdentity)identity;
			return parseFunc(claimsIdentity.FindFirst(ClaimTypes.UserId).Value);
		}

		public static string GetSecurityStamp(this IIdentity identity)
		{
			if (!identity.IsAuthenticated)
			{
				throw new InvalidOperationException();
			}

			var claimsIdentity = (ClaimsIdentity)identity;
			return claimsIdentity.FindFirst(ClaimTypes.SecurityStamp).Value;
		}

		public static void SetUserId(this ClaimsIdentity claimsIdentity, string userId)
		{
			claimsIdentity.AddClaim(new Claim(ClaimTypes.UserId, userId));
		}

		public static void SetSecurityStamp(this ClaimsIdentity claimsIdentity, string securityStamp)
		{
			claimsIdentity.AddClaim(new Claim(ClaimTypes.SecurityStamp, securityStamp));
		}

		public static string GetEmail(this IIdentity identity)
		{
			if (!identity.IsAuthenticated)
			{
				throw new InvalidOperationException();
			}

			var claimsIdentity = (ClaimsIdentity)identity;
			return claimsIdentity.FindFirst(ClaimValueTypes.Email).Value;
		}
	}
}