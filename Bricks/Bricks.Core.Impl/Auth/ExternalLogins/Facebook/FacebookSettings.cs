#region

using System.Configuration;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.Facebook
{
	/// <summary>
	/// Реализация <see cref="IFacebookSettings" /> при получении настроек из файла конфигурации.
	/// </summary>
	internal sealed class FacebookSettings : ConfigurationSection, IFacebookSettings
	{
		private const string AppIdName = "appId";
		private const string AppSecretName = "appSecret";

		#region Implementation of IFacebookSettings

		/// <summary>
		/// Идентификатор приложения.
		/// </summary>
		[ConfigurationProperty(AppIdName, IsRequired = true)]
		public string AppId
		{
			get { return (string) this[AppIdName]; }
		}

		/// <summary>
		/// Секретный ключ приложения.
		/// </summary>
		[ConfigurationProperty(AppSecretName, IsRequired = true)]
		public string AppSecret
		{
			get { return (string) this[AppSecretName]; }
		}

		#endregion
	}
}