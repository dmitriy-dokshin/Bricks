#region

using Bricks.Core.Modularity.Implementation;
using Bricks.Sync.Implementation;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Sync
{
	public sealed class Module : ModuleBase
	{
		#region Overrides of ModuleBase

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="container">The <see cref="IUnityContainer" /> that is used to store application dependencies.</param>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		public override void Initialize(IUnityContainer container, IUnityContainer args)
		{
			container.RegisterType<IInterlockedHelper, InterlockedHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<ISyncFactory, SyncFactory>(new ContainerControlledLifetimeManager());
		}

		#endregion
	}
}