#region

using System.Globalization;
using System.Resources;

#endregion

namespace Bricks.Core.Resources.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IResourceManager" />.
	/// </summary>
	internal sealed class ResourceManagerImpl : IResourceManager
	{
		private readonly ResourceManager _resourceManager;

		public ResourceManagerImpl(ResourceManager resourceManager)
		{
			_resourceManager = resourceManager;
		}

		#region Implementation of IResourceManager

		/// <summary>
		/// Получает локализованную строку, соответствующую культуре <paramref name="culture" />,
		/// по имени <paramref name="name" />.
		/// </summary>
		/// <param name="name">Имя строки в ресурсе.</param>
		/// <param name="culture">Культура.</param>
		/// <returns>Локализованная строка.</returns>
		public string GetString(string name, CultureInfo culture = null)
		{
			return _resourceManager.GetString(name, culture);
		}

		#endregion
	}
}