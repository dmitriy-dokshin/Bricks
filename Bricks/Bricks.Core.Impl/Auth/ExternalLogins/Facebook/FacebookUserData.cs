#region

using Bricks.Core.Auth.ExternalLogins;

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	internal sealed class FacebookUserData : IExternalLoginData
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("about")]
		public string About { get; set; }

		public string ImageUrl { get; set; }

		string IExternalLoginData.Key
		{
			get
			{
				return Id;
			}
		}
	}
}