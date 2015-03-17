#region

using System.Diagnostics;

using Bricks.Core.Modularity;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Samples.ConsoleApp
{
	internal sealed class Module : ModuleBase
	{
		#region Overrides of ModuleBase

		public override void Initialize(IUnityContainer container, IUnityContainer args)
		{
			base.Initialize(container, args);
			Trace.WriteLine("Bricks.Samples.ConsoleApp initialization completed.");
		}

		#endregion
	}
}