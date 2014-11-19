#region

using Newtonsoft.Json;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Часть сообщения в ответе <see cref="SmsResult" />.
	/// </summary>
	internal sealed class MessagePartDto
	{
		/// <summary>
		/// The return code.
		/// </summary>
		[JsonProperty("status")]
		public NexmoResponseCode Status { get; set; }

		/// <summary>
		/// The ID of the message that was submitted (8 to 16 characters).
		/// </summary>
		[JsonProperty("message-id")]
		public string MessageId { get; set; }

		/// <summary>
		/// The recipient number.
		/// </summary>
		[JsonProperty("to")]
		public string To { get; set; }

		/// <summary>
		/// If you set a custom reference during your request, this will return that value.
		/// </summary>
		[JsonProperty("client-ref")]
		public string ClientReg { get; set; }

		/// <summary>
		/// The remaining account balance expressed in EUR.
		/// </summary>
		[JsonProperty("remaining-balance")]
		public decimal RemainingBalance { get; set; }

		/// <summary>
		/// The price charged (EUR) for the submitted message.
		/// </summary>
		[JsonProperty("message-price")]
		public decimal MessagePrice { get; set; }

		/// <summary>
		/// Identifier of a mobile network MCCMNC.
		/// </summary>
		[JsonProperty("network")]
		public string Network { get; set; }

		/// <summary>
		/// If an error occurred, this will explain in readable terms the error encountered.
		/// </summary>
		[JsonProperty("error-text")]
		public string ErrorText { get; set; }
	}
}