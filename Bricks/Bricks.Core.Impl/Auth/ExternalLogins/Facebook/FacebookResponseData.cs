#region

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	internal sealed class FacebookResponseData<T>
	{
		[JsonProperty("Data")]
		public T Data { get; set; }
	}
}