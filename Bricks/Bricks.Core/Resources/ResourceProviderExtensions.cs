#region

using System;

#endregion

namespace Bricks.Core.Resources
{
	/// <summary>
	/// Содрежит методы расширения для <see cref="IResourceProvider" />.
	/// </summary>
	public static class ResourceProviderExtensions
	{
		/// <summary>
		/// Получает менеджера ресурсов по имени типа сгенерированного класса ресурсов.
		/// </summary>
		/// <param name="resourceProvider">Поставщик ресурсов.</param>
		/// <param name="resourceTypeName">Полное имя типа сгенерированного класса ресурсов.</param>
		/// <returns>Менеджер ресурсов.</returns>
		public static IResourceManager GetResourceManager(this IResourceProvider resourceProvider, string resourceTypeName)
		{
			var type = Type.GetType(resourceTypeName, true);
			return resourceProvider.GetResourceManager(type);
		}
	}
}