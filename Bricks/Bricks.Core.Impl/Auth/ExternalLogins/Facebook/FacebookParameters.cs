#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	internal class FacebookParameters
	{
		protected FacebookParameters(string accessToken, int pretty = 0, string format = "json", string locale = "ru_RU")
		{
			AccessToken = accessToken;
			Format = format;
			Pretty = pretty;
			Locale = locale;
		}

		[JsonProperty("access_token")]
		public string AccessToken { get; private set; }

		[JsonProperty("format")]
		public string Format { get; private set; }

		[JsonProperty("pretty")]
		public int Pretty { get; private set; }

		[JsonProperty("locale")]
		public string Locale { get; private set; }
	}
}