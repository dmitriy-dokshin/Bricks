#region

using System;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.Modularity
{
	/// <summary>
	/// Represents a module of an application.
	/// </summary>
	public interface IModule : IDisposable
	{
		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="container">The <see cref="IUnityContainer" /> that is used to store application dependencies.</param>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		void Initialize(IUnityContainer container, IUnityContainer args);
	}
}