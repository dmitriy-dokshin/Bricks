#region

using System.Linq;

using Bricks.Core.Configuration;
using Bricks.Core.Disposing;
using Bricks.Core.IoC;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

#endregion

namespace Bricks.Core.Modularity
{
	/// <summary>
	/// Базовый класс приложения.
	/// </summary>
	public abstract class ApplicationBase : DisposableBase, IApplication
	{
		/// <summary>
		/// Ключ настроек модулей.
		/// </summary>
		protected const string MODULARITY_SETTINGS_KEY = "modularitySettings";

		private readonly IUnityContainer _container;

		protected ApplicationBase(IUnityContainer container = null)
		{
			_container = container ?? new UnityContainerFixed();
		}

		#region Overrides of DisposableBase

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">
		/// <c>true</c> if method is called from <see cref="DisposableBase.Dispose" />; <c>false</c> if method is called by
		/// finalizer.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed && disposing)
			{
				var configurationManager = _container.Resolve<IConfigurationManager>();

				var modularitySettings = configurationManager.GetSettings<IModularitySettings>(MODULARITY_SETTINGS_KEY);
				foreach (IModuleSettings moduleSettings in modularitySettings.Modules.OrderByDescending(x => x.Order))
				{
					if (moduleSettings.Type != null)
					{
						var module = _container.Resolve<IModule>(moduleSettings.Name);
						module.Dispose();
					}
				}

				_container.Dispose();
			}

			base.Dispose(disposing);
		}

		#endregion

		protected abstract void Initialize(IUnityContainer container, IUnityContainer args);

		#region Implementation of IApplication

		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		public virtual void Initialize(IUnityContainer args = null)
		{
			if (args == null)
			{
				args = new UnityContainer();
			}

			_container.LoadConfiguration();

			var configurationManager = _container.Resolve<IConfigurationManager>();

			var modularitySettings = configurationManager.GetSettings<IModularitySettings>(MODULARITY_SETTINGS_KEY);
			foreach (IModuleSettings moduleSettings in modularitySettings.Modules.OrderBy(x => x.Order))
			{
				_container.RegisterType(typeof(IModule), moduleSettings.Type, moduleSettings.Name, new ContainerControlledLifetimeManager());
				var module = _container.Resolve<IModule>(moduleSettings.Name);
				using (args.CreateChildContainer())
				{
					args.RegisterInstance(moduleSettings);
					module.Initialize(_container, args);
				}
			}

			ServiceLocator.SetLocatorProvider(() => _container.Resolve<IServiceLocator>());
			Initialize(_container, args);
		}

		IServiceLocator IApplication.ServiceLocator
		{
			get
			{
				return _container.Resolve<IServiceLocator>();
			}
		}

		#endregion
	}
}