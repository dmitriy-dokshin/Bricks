using Bricks.Core.Disposing.Implementation;
using Bricks.Core.Modularity;
using Bricks.Sync.Implementation;

using Microsoft.Practices.Unity;

namespace Bricks.Sync
{
	public sealed class Module : DisposableBase, IModule
	{
		#region Implementation of IModule

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="container">The <see cref="IUnityContainer" /> that is used to store application dependencies.</param>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		public void Initialize(IUnityContainer container, IUnityContainer args)
		{
			container.RegisterType<IInterlockedHelper, InterlockedHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<ILockAsync, LockAsync>();
		}

		#endregion
	}
}