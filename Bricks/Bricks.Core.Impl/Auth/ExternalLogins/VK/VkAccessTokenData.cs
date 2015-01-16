#region

using System;

using Bricks.Core.Auth.ExternalLogins;

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	public sealed class VkAccessTokenData : IAccessTokenData
	{
		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("user_id")]
		public string UserId { get; set; }

		#region Implementation of IAccessTokenData

		TimeSpan IAccessTokenData.ExpiresIn
		{
			get
			{
				return TimeSpan.FromSeconds(ExpiresIn);
			}
		}

		#endregion
	}
}