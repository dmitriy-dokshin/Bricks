#region

using System;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.Modularity
{
	/// <summary>
	/// Represents a modular application.
	/// </summary>
	public interface IApplication : IDisposable
	{
		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		void Initialize(IUnityContainer args = null);
	}
}