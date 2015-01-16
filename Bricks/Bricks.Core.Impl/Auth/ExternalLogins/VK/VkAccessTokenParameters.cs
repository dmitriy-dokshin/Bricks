#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	internal sealed class VkAccessTokenParameters
	{
		public VkAccessTokenParameters(string clientId, string clientSecret, string code, string redirectUri)
		{
			ClientId = clientId;
			ClientSecret = clientSecret;
			Code = code;
			RedirectUri = redirectUri;
		}

		[JsonProperty("client_id")]
		public string ClientId { get; private set; }

		[JsonProperty("client_secret")]
		public string ClientSecret { get; private set; }

		[JsonProperty("code")]
		public string Code { get; private set; }

		[JsonProperty("redirect_uri")]
		public string RedirectUri { get; private set; }
	}
}