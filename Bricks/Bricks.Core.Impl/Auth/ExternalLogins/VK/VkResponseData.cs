using Newtonsoft.Json;

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	internal sealed class VkResponseData<T>
	{
		[JsonProperty("response")]
		public T Response { get; set; }
	}
}