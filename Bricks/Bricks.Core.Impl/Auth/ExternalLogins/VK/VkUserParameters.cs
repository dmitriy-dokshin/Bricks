#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	internal sealed class VkUserParameters : VkParameters
	{
		public VkUserParameters(string accessToken, string fields = "email,photo_max,about")
			: base(accessToken)
		{
			Fields = fields;
		}

		[JsonProperty("fields")]
		public string Fields { get; internal set; }
	}
}