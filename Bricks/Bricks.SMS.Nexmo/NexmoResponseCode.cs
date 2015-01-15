#region

using Bricks.Core.Enum;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Коды статусов отправки сообщений.
	/// </summary>
	[EnumResource(ResourceType = typeof(Resources))]
	public enum NexmoResponseCode
	{
		Success = 0,
		Throttled = 1,
		MissingParams = 2,
		InvalidParams = 3,
		InvalidCredentials = 4,
		InternalError = 5,
		InvalidMessage = 6,
		NumberBarred = 7,
		PartnerAccountBarred = 8,
		PartnerQuotaExceeded = 9,
		AccountNotEnabledForREST = 11,
		MessageTooLong = 12,
		CommunicationFailed = 13,
		InvalidSignature = 14,
		InvalidSenderAddress = 15,
		InvalidTTL = 16,
		FacilityNotAllowed = 19,
		InvalidMessageClass = 20
	}
}