#region

using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Resources;

using Bricks.Core.Resources;
using Bricks.Core.Sync;

#endregion

namespace Bricks.Core.Impl.Resources
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IResourceProvider" />.
	/// </summary>
	internal sealed class ResourceProvider : IResourceProvider
	{
		private readonly IInterlockedHelper _interlockedHelper;
		private IImmutableDictionary<ResourceManagerKey, IResourceManager> _resourceManagers;

		public ResourceProvider(IInterlockedHelper interlockedHelper)
		{
			_interlockedHelper = interlockedHelper;
			_resourceManagers = ImmutableDictionary.Create<ResourceManagerKey, IResourceManager>();
		}

		private sealed class ResourceManagerKey
		{
			private readonly Assembly _assembly;
			private readonly string _baseName;

			public ResourceManagerKey(string baseName, Assembly assembly)
			{
				_baseName = baseName;
				_assembly = assembly;
			}

			public override bool Equals(object obj)
			{
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}

				var resourceManagerKey = (ResourceManagerKey)obj;
				bool result = _baseName.Equals(resourceManagerKey._baseName) && _assembly.Equals(resourceManagerKey._assembly);
				return result;
			}

			public override int GetHashCode()
			{
				return _baseName.GetHashCode() | _assembly.GetHashCode();
			}
		}

		#region Implementation of IResourceProvider

		/// <summary>
		/// Получает менеджера ресурсов по типу сгенерированного класса ресурсов.
		/// </summary>
		/// <param name="resourceType">Тип сгенерированного класса ресурсов.</param>
		/// <returns>Менеджер ресурсов.</returns>
		public IResourceManager GetResourceManager(Type resourceType)
		{
			string baseName = resourceType.FullName;
			Assembly assembly = resourceType.Assembly;
			return GetResourceManager(baseName, assembly);
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
			var resourceManagerKey = new ResourceManagerKey(baseName, assembly);
			return _interlockedHelper.CompareExchange(ref _resourceManagers, x =>
				{
					IResourceManager result;
					IImmutableDictionary<ResourceManagerKey, IResourceManager> newValue;
					if (!_resourceManagers.TryGetValue(resourceManagerKey, out result))
					{
						var resourceManager = new ResourceManager(baseName, assembly);
						result = new ResourceManagerImpl(resourceManager);
						newValue = x.Add(resourceManagerKey, result);
					}
					else
					{
						newValue = x;
					}

					return _interlockedHelper.CreateChangeResult(newValue, result);
				});
		}

		#endregion
	}
}