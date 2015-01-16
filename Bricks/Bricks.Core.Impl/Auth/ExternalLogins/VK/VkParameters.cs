#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	internal abstract class VkParameters
	{
		protected VkParameters(string accessToken, string version = "5.8", string language = "ru")
		{
			AccessToken = accessToken;
			Version = version;
			Language = language;
		}

		[JsonProperty("access_token")]
		public string AccessToken { get; private set; }

		[JsonProperty("v")]
		public string Version { get; internal set; }

		[JsonProperty("lang")]
		public string Language { get; internal set; }
	}
}