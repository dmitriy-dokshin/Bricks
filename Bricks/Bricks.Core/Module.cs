#region

using Bricks.Core.Disposing;
using Bricks.Core.Disposing.Implementation;
using Bricks.Core.Modularity;
using Bricks.Core.Seams;
using Bricks.Core.Seams.Implementation;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core
{
	public class Module : DisposableBase, IModule
	{
		#region Implementation of IModule

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="container">The <see cref="IUnityContainer" /> that is used to store application dependencies.</param>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		public void Initialize(IUnityContainer container, IUnityContainer args)
		{
			container.RegisterType<IRandomProvider, RandomProvider>(new ContainerControlledLifetimeManager());
		}

		#endregion
	}
}