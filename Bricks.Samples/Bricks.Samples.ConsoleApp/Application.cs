#region

using System.Diagnostics;

using Bricks.Core.Modularity;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Samples.ConsoleApp
{
	internal sealed class Application : ApplicationBase
	{
		#region Overrides of ApplicationBase

		protected override void Initialize(IUnityContainer container, IUnityContainer args)
		{
			Trace.WriteLine("Application initialization completed.");
		}

		#endregion
	}
}