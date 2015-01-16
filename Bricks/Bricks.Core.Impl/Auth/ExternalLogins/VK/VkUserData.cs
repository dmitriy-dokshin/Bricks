#region

using Bricks.Core.Auth.ExternalLogins;

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	internal sealed class VkUserData : IExternalLoginData
	{
		[JsonProperty("photo_max")]
		public string ImageUrl { get; set; }

		[JsonProperty("id")]
		public string Key { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("about")]
		public string About { get; set; }
	}
}