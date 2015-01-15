#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	public sealed class FacebookUserPictureData
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("is_silhouette")]
		public bool IsDefault { get; set; }
	}
}