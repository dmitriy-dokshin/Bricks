#region

using System.Collections.Generic;

using Newtonsoft.Json;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Результат отправки сообщения.
	/// </summary>
	internal sealed class SmsResult
	{
		/// <summary>
		/// The number of parts the message was split into.
		/// </summary>
		[JsonProperty("message-count")]
		public int MessagesCount { get; set; }

		/// <summary>
		/// An array containing objects for each message part.
		/// </summary>
		[JsonProperty("messages")]
		public IReadOnlyCollection<MessagePartDto> Messages { get; set; }
	}
}