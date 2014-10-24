#region

using System.Collections.Generic;
using System.Configuration;

#endregion

namespace Bricks.Core.Modularity.Implementation
{
	/// <summary>
	/// Секция настроек модульности приложения.
	/// </summary>
	internal sealed class ModularityConfigurationSection : ConfigurationSection, IModularitySettings
	{
		private const string ModulesName = "modules";

		[ConfigurationProperty(ModulesName, IsRequired = true)]
		[ConfigurationCollection(typeof(ModuleConfigurationElementCollection))]
		public ModuleConfigurationElementCollection Modules
		{
			get
			{
				return (ModuleConfigurationElementCollection)this[ModulesName];
			}
		}

		#region Implementation of IModularitySettings

		/// <summary>
		/// Коллекция модулей приложения.
		/// </summary>
		IReadOnlyCollection<IModuleSettings> IModularitySettings.Modules
		{
			get
			{
				return Modules;
			}
		}

		#endregion
	}
}