#region

using System;
using System.Configuration;

#endregion

namespace Bricks.Helpers.Regex.Implementation
{
	/// <summary>
	/// Секция настроек регулярных выражений.
	/// </summary>
	internal sealed class RegexSettings : ConfigurationSection, IRegexSettings
	{
		private const string EmailName = "email";
		private const string PhoneName = "phone";
		private const string MatchTimeoutName = "matchTimeout";

		[ConfigurationProperty(EmailName, IsRequired = true)]
		public EmailRegexSettings Email
		{
			get { return (EmailRegexSettings) this[EmailName]; }
		}

		[ConfigurationProperty(PhoneName, IsRequired = true)]
		public PhoneRegexSettings Phone
		{
			get { return (PhoneRegexSettings) this[PhoneName]; }
		}

		[ConfigurationProperty(MatchTimeoutName, IsRequired = false, DefaultValue = 1000d)]
		public double MatchTimeout
		{
			get { return (double) this[MatchTimeoutName]; }
		}

		#region Implementation of IRegexSettings

		/// <summary>
		/// Настройки email'а.
		/// </summary>
		IEmailRegexSettings IRegexSettings.Email
		{
			get { return Email; }
		}

		/// <summary>
		/// Настройки номера телефона.
		/// </summary>
		IPhoneRegexSettings IRegexSettings.Phone
		{
			get { return Phone; }
		}

		/// <summary>
		/// Таймаут разбора в милисекундах.
		/// </summary>
		TimeSpan IRegexSettings.MatchTimeout
		{
			get { return TimeSpan.FromMilliseconds(MatchTimeout); }
		}

		#endregion
	}
}