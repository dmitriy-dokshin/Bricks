#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	public sealed class FacebookResponseData<T>
	{
		[JsonProperty("Data")]
		public T Data { get; set; }
	}
}