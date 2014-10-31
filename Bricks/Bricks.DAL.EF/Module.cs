#region

using Bricks.Core.Modularity.Implementation;
using Bricks.DAL.Repository;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.DAL.EF
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
			container.RegisterType<IRepository, Repository>();
		}

		#endregion
	}
}