#region

using Newtonsoft.Json;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Параметры запроса отправки SMS.
	/// </summary>
	internal sealed class SmsParameters : NexmoParametersBase
	{
		public SmsParameters(string to, string text)
		{
			To = to;
			Text = text;
			Type = "unicode";
		}

		/// <summary>
		/// Required. Sender address may be alphanumeric (Ex: from=MyCompany20). Restrictions may apply, depending on the
		/// destination.
		/// </summary>
		[JsonProperty("from")]
		public string From { get; private set; }

		/// <summary>
		/// Required. Mobile number in international format, and one recipient per request. Ex: to=447525856424 or
		/// to=00447525856424 when sending to UK.
		/// </summary>
		[JsonProperty("to")]
		public string To { get; private set; }

		/// <summary>
		/// Optional. This can be omitted for text (default), but is required when sending a Binary (binary), WAP Push (wappush),
		/// Unicode message (unicode), vcal (vcal) or vcard (vcard).
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }

		/// <summary>
		/// Required when type='text'. Body of the text message (with a maximum length of 3200 characters), UTF-8 and URL encoded
		/// value. Ex: "Déjà vu" content would be "D%c3%a9j%c3%a0+vu".
		/// </summary>
		[JsonProperty("text")]
		public string Text { get; set; }

		/// <summary>
		/// Optional. Set to 1 if you want to receive a delivery report (DLR) for this request. Make sure to configure your
		/// "Callback URL" in your "API Settings".
		/// </summary>
		[JsonProperty("status-report-req")]
		public int? StatusReportReq { get; set; }

		/// <summary>
		/// Optional. Include any reference string for your reference. Useful for your internal reports (40 characters max).
		/// </summary>
		[JsonProperty("client-ref")]
		public string ClientRef { get; set; }

		/// <summary>
		/// Optional. Force the recipient network operator MCCMNC, make sure to supply the correct information otherwise the
		/// message won't be delivered.
		/// </summary>
		[JsonProperty("network-code")]
		public string NetworkCode { get; set; }

		/// <summary>
		/// Optional. vcard text body correctly formatted.
		/// </summary>
		[JsonProperty("vcad")]
		public string Vcad { get; set; }

		/// <summary>
		/// Optional. vcal text body correctly formatted.
		/// </summary>
		[JsonProperty("vcal")]
		public string Vcal { get; set; }

		/// <summary>
		/// Optional. Message life span in milliseconds.
		/// </summary>
		[JsonProperty("ttl")]
		public string Ttl { get; set; }

		/// <summary>
		/// Optional. Set to 0 for Flash SMS.
		/// </summary>
		[JsonProperty("message-class")]
		public int? MessageClass { get; set; }

		#region Overrides of NexmoParametersBase

		protected override void Initialize(INexmoSettings nexmoSettings)
		{
			base.Initialize(nexmoSettings);
			From = nexmoSettings.SenderId;
		}

		#endregion
	}
}