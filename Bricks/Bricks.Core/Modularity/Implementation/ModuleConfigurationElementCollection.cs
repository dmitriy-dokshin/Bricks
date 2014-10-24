#region

using Bricks.Core.Configuration;
using Bricks.Core.Configuration.Implementation;

#endregion

namespace Bricks.Core.Modularity.Implementation
{
	/// <summary>
	/// Коллекция настроек модулей.
	/// </summary>
	internal sealed class ModuleConfigurationElementCollection : ConfigurationElementCollectionAdapter<ModuleConfigurationElement, string>
	{
	}
}