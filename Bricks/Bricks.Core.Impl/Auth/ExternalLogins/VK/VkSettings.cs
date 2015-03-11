#region

using System.Configuration;

using Bricks.Core.Auth.ExternalLogins;

#endregion

namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	/// <summary>
	/// Реализация <see cref="IVkSettings" /> при получении настроек из файла конфигурации.
	/// </summary>
	internal sealed class VkSettings : ConfigurationSection, IVkSettings
	{
		private const string ClientIdName = "clientId";
		private const string ClientSecretName = "clientSecret";

		#region Implementation of IVkSettings

		/// <summary>
		/// Идентификатор приложения.
		/// </summary>
		[ConfigurationProperty(ClientIdName, IsRequired = true)]
		public string ClientId
		{
			get { return (string) this[ClientIdName]; }
		}

		/// <summary>
		/// Секретный ключ приложения.
		/// </summary>
		[ConfigurationProperty(ClientSecretName, IsRequired = true)]
		public string ClientSecret
		{
			get { return (string) this[ClientSecretName]; }
		}

		#endregion
	}
}