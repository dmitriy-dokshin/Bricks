#region

using Bricks.Core.Disposing.Implementation;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

#endregion

namespace Bricks.Core.Modularity.Implementation
{
	/// <summary>
	/// Базовый класс модуля.
	/// </summary>
	public abstract class ModuleBase : DisposableBase, IModule
	{
		#region Implementation of IModule

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="container">The <see cref="IUnityContainer" /> that is used to store application dependencies.</param>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		public virtual void Initialize(IUnityContainer container, IUnityContainer args)
		{
			var moduleSettings = args.Resolve<IModuleSettings>();
			container.LoadConfiguration(moduleSettings.Name);
		}

		#endregion
	}
}