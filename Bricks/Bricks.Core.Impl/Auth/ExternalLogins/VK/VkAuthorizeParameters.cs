#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	internal sealed class VkAuthorizeParameters
	{
		public VkAuthorizeParameters(string clientId, string scope, string redirectUri, string responseType = "code")
		{
			ClientId = clientId;
			Scope = scope;
			RedirectUri = redirectUri;
			ResponseType = responseType;
		}

		[JsonProperty("client_id")]
		public string ClientId { get; private set; }

		[JsonProperty("scope")]
		public string Scope { get; private set; }

		[JsonProperty("redirect_uri")]
		public string RedirectUri { get; private set; }

		[JsonProperty("response_type")]
		public string ResponseType { get; private set; }
	}
}