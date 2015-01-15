#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	internal sealed class FacebookUserParameters : FacebookParameters
	{
		public FacebookUserParameters(string accessToken, string fields = "email,first_name,last_name,about")
			: base(accessToken)
		{
			Fields = fields;
		}

		[JsonProperty("fields")]
		public string Fields { get; private set; }
	}
}