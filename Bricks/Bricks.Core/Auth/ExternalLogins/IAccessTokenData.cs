#region

using System;

#endregion

namespace Bricks.Core.Auth.ExternalLogins
{
	public interface IAccessTokenData
	{
		string AccessToken { get; }

		TimeSpan ExpiresIn { get; }

		string UserId { get; }
	}
}