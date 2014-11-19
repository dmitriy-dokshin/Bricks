#region

using System;
using System.Reflection;

#endregion

namespace Bricks.Core.Resources
{
	/// <summary>
	/// Поставщик ресурсов.
	/// </summary>
	public interface IResourceProvider
	{
		/// <summary>
		/// Получает менеджера ресурсов по типу сгенерированного класса ресурсов.
		/// </summary>
		/// <param name="resourceType">Тип сгенерированного класса ресурсов.</param>
		/// <returns>Менеджер ресурсов.</returns>
		IResourceManager GetResourceManager(Type resourceType);

		/// <summary>
		/// Получает менеджера ресурсов по названию <paramref name="baseName" />
		/// встроенного в сборку <paramref name="assembly" /> ресурса.
		/// </summary>
		/// <param name="baseName">Название ресурса.</param>
		/// <param name="assembly">Сборка.</param>
		/// <returns>Менеджер ресурсов.</returns>
		IResourceManager GetResourceManager(string baseName, Assembly assembly);
	}
}