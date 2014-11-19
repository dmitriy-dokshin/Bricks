#region

using System.Configuration;

using Bricks.Core.Configuration;

#endregion

namespace Bricks.Web
{
	/// <summary>
	/// URL web-сервиса с типизированными параметрами.
	/// </summary>
	public class ServiceUrl : ConfigurationElement, IKeyConfigurationElement<string>
	{
		private const string TypeName = "type";
		private const string UrlName = "url";

		[ConfigurationProperty(TypeName, IsRequired = true)]
		public string Type
		{
			get
			{
				return (string)this[TypeName];
			}
		}

		[ConfigurationProperty(UrlName, IsRequired = true)]
		public string Url
		{
			get
			{
				return (string)this[UrlName];
			}
		}

		#region Implementation of IKeyConfigurationElement<out string>

		/// <summary>
		/// Ключ элемента.
		/// </summary>
		string IKeyConfigurationElement<string>.Key
		{
			get
			{
				return Type;
			}
		}

		#endregion
	}
}