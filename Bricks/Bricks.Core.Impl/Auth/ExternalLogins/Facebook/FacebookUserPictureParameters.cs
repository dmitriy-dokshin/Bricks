#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	internal sealed class FacebookUserPictureParameters : FacebookParameters
	{
		public FacebookUserPictureParameters(string accessToken, bool redirect = false, int? height = 300, int? width = 300, string type = "large")
			: base(accessToken)
		{
			Redirect = redirect;
			Height = height;
			Width = width;
			Type = type;
		}

		[JsonProperty("redirect")]
		public bool Redirect { get; private set; }

		[JsonProperty("height")]
		public int? Height { get; private set; }

		[JsonProperty("width")]
		public int? Width { get; private set; }

		[JsonProperty("type")]
		public string Type { get; private set; }
	}
}