#region

using Bricks.Core.Configuration;

#endregion

namespace Bricks.Core.Impl.Modularity
{
	/// <summary>
	/// Коллекция настроек модулей.
	/// </summary>
	internal sealed class ModuleConfigurationElementCollection : ConfigurationElementCollectionAdapter<ModuleConfigurationElement, string>
	{
	}
}