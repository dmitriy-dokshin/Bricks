#region

using System;

using Bricks.Core.Configuration;

using Microsoft.Practices.Unity;

using Newtonsoft.Json;

#endregion

namespace Bricks.SMS.Nexmo
{
	/// <summary>
	/// Базовые параметры запросов к Nexmo.
	/// </summary>
	public abstract class NexmoParametersBase
	{
		/// <summary>
		/// Ключ приложения.
		/// </summary>
		[JsonProperty("api_key")]
		public string ApiKey { get; private set; }

		/// <summary>
		/// Секретный ключ.
		/// </summary>
		[JsonProperty("api_secret")]
		public string ApiSecret { get; private set; }

		/// <summary>
		/// Адрес сервиса, соответствующего типу данных параметров.
		/// </summary>
		[JsonIgnore]
		public Uri ServiceUrl { get; private set; }

		[InjectionMethod]
		public void Initialize(IConfigurationManager configurationManager)
		{
			var nexmoSettings = configurationManager.GetSettings<INexmoSettings>(Consts.NexmoSettingsKey);
			Initialize(nexmoSettings);
		}

		protected virtual void Initialize(INexmoSettings nexmoSettings)
		{
			ApiKey = nexmoSettings.Key;
			ApiSecret = nexmoSettings.Secret;
			ServiceUrl = nexmoSettings.ServiceUrls[GetType()];
		}
	}
}