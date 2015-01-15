#region

using System;
using System.Reflection;
using System.Resources;

using Bricks.Core.Resources;

#endregion

namespace Bricks.Core.Impl.Resources
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IResourceProvider" />.
	/// </summary>
	internal sealed class ResourceProvider : IResourceProvider
	{
		#region Implementation of IResourceProvider

		/// <summary>
		/// Получает менеджера ресурсов по типу сгенерированного класса ресурсов.
		/// </summary>
		/// <param name="resourceType">Тип сгенерированного класса ресурсов.</param>
		/// <returns>Менеджер ресурсов.</returns>
		public IResourceManager GetResourceManager(Type resourceType)
		{
			var propertyInfo = resourceType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic);
			var resourceManager = propertyInfo.GetValue(null);
			return new ResourceManagerImpl((ResourceManager)resourceManager);
		}

		/// <summary>
		/// Получает менеджера ресурсов по названию <paramref name="baseName" />
		/// встроенного в сборку <paramref name="assembly" /> ресурса.
		/// </summary>
		/// <param name="baseName">Название ресурса.</param>
		/// <param name="assembly">Сборка.</param>
		/// <returns>Менеджер ресурсов.</returns>
		public IResourceManager GetResourceManager(string baseName, Assembly assembly)
		{
			var resourceManager = new ResourceManager(baseName, assembly);
			return new ResourceManagerImpl(resourceManager);
		}

		#endregion
	}
}