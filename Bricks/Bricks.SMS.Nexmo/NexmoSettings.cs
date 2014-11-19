#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Bricks.Core.Configuration.Implementation;
using Bricks.Core.Resources;
using Bricks.Web;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Реализация <see cref="INexmoSettings" /> для чтения настроек из файла конфигурации.
	/// </summary>
	internal sealed class NexmoSettings : ConfigurationSection, INexmoSettings
	{
		private const string KeyName = "key";
		private const string SecretName = "secret";
		private const string BaseUrlName = "baseUrl";
		private const string ServiceUrlsName = "serviceUrls";
		private const string SenderIdResourceTypeName = "senderIdResourceType";
		private const string SenderIdResourceNameName = "senderIdResourceName";

		private Uri _baseUrl;
		private IReadOnlyDictionary<Type, Uri> _serviceUrls;

		[ConfigurationProperty(ServiceUrlsName, IsRequired = true)]
		[ConfigurationCollection(typeof(ConfigurationElementCollectionAdapter<ServiceUrl, string>))]
		public ConfigurationElementCollectionAdapter<ServiceUrl, string> ServiceUrls
		{
			get
			{
				return (ConfigurationElementCollectionAdapter<ServiceUrl, string>)this[ServiceUrlsName];
			}
		}

		[ConfigurationProperty(BaseUrlName, IsRequired = true)]
		public string BaseUrl
		{
			get
			{
				return (string)this[BaseUrlName];
			}
		}

		[ConfigurationProperty(SenderIdResourceTypeName, IsRequired = true)]
		public string SenderIdResourceType
		{
			get
			{
				return (string)this[SenderIdResourceTypeName];
			}
		}

		[ConfigurationProperty(SenderIdResourceNameName, IsRequired = true)]
		public string SenderIdResourceName
		{
			get
			{
				return (string)this[SenderIdResourceNameName];
			}
		}

		#region Implementation of INexmoSettings

		/// <summary>
		/// Ключ приложения.
		/// </summary>
		[ConfigurationProperty(KeyName, IsRequired = true)]
		public string Key
		{
			get
			{
				return (string)this[KeyName];
			}
		}

		/// <summary>
		/// Секретный ключ.
		/// </summary>
		[ConfigurationProperty(SecretName, IsRequired = true)]
		public string Secret
		{
			get
			{
				return (string)this[SecretName];
			}
		}

		/// <summary>
		/// Базовый URL.
		/// </summary>
		Uri INexmoSettings.BaseUrl
		{
			get
			{
				return _baseUrl;
			}
		}

		/// <summary>
		/// Адреса сервисов.
		/// </summary>
		IReadOnlyDictionary<Type, Uri> INexmoSettings.ServiceUrls
		{
			get
			{
				return _serviceUrls;
			}
		}

		/// <summary>
		/// Идентификатор отправителя.
		/// </summary>
		public string SenderId { get; private set; }

		#endregion

		[InjectionMethod]
		public void Initialize(IResourceProvider resourceProvider)
		{
			_baseUrl = new Uri(BaseUrl);
			_serviceUrls = ServiceUrls.ToDictionary(x => Type.GetType(x.Type, true), x => new Uri(x.Url, UriKind.RelativeOrAbsolute).ToAbsoluteIfNot(_baseUrl));
			SenderId = resourceProvider.GetResourceManager(SenderIdResourceType).GetString(SenderIdResourceName);
		}
	}
}