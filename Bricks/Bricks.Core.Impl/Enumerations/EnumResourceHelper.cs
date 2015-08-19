#region

using System;
using System.Globalization;
using System.Reflection;
using Bricks.Core.Enumerations;
using Bricks.Core.Resources;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IEnumResourceHelper" />.
	/// </summary>
	internal sealed class EnumResourceHelper : IEnumResourceHelper
	{
		/// <summary>
		/// Шаблон ключа названия перечисления в ресурсе.
		/// <para>{0}: название типа перечисления.</para>
		/// </summary>
		private const string EnumNameResourceKeyTemplate = "{0}";

		/// <summary>
		/// Шаблон ключа метаданных перечисления в ресурсе.
		/// <para>{0}: название типа перечисления.</para>
		/// <para>{1}: ключ метаданных.</para>
		/// </summary>
		private const string EnumMetadataResourceKeyTemplate = "{0}_{1}";

		/// <summary>
		/// Шаблон ключа названия значения перечисления в ресурсе.
		/// <para>{0}: название типа перечисления.</para>
		/// <para>{1}: название значения перечисления.</para>
		/// </summary>
		private const string EnumValueNameResourceKeyTemplate = "{0}_{1}";

		/// <summary>
		/// Шаблон ключа метаданных значения перечисления в ресурсе.
		/// <para>{0}: название типа перечисления.</para>
		/// <para>{1}: название значения перечисления.</para>
		/// <para>{2}: ключ метаданных.</para>
		/// </summary>
		private const string EnumValueMetadataResourceKeyTemplate = "{0}_{1}_{2}";

		private readonly Type _enumType;
		private readonly IResourceProvider _resourceProvider;

		public EnumResourceHelper(Type enumType, IResourceProvider resourceProvider)
		{
			_enumType = enumType;
			_resourceProvider = resourceProvider;
		}

		private IResourceManager GetResourceManager()
		{
			IResourceManager resourceManager = null;
			var enumResourceAttribute =
				_enumType.GetCustomAttribute<EnumResourceAttribute>(false);
			if (enumResourceAttribute != null)
			{
				if (!string.IsNullOrEmpty(enumResourceAttribute.BaseName))
				{
					Assembly resourceAssembly =
						!string.IsNullOrEmpty(enumResourceAttribute.AssemblyString)
							? Assembly.Load(enumResourceAttribute.AssemblyString)
							: _enumType.Assembly;
					resourceManager = _resourceProvider.GetResourceManager(enumResourceAttribute.BaseName, resourceAssembly);
				}
				else
				{
					Type resourceType = enumResourceAttribute.ResourceType;
					if (resourceType == null && !string.IsNullOrEmpty(enumResourceAttribute.ResourceTypeFullName))
					{
						resourceType = Type.GetType(enumResourceAttribute.ResourceTypeFullName);
					}

					if (resourceType != null)
					{
						resourceManager = _resourceProvider.GetResourceManager(resourceType);
					}
				}
			}
			return resourceManager;
		}

		#region Implementation of IEnumResourceHelper

		/// <summary>
		/// Получает локализованное название типа перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное название типа перечисления.</returns>
		public string GetEnumName(CultureInfo cultureInfo)
		{
			if (cultureInfo != null && cultureInfo.Equals(CultureInfo.InvariantCulture))
			{
				return _enumType.Name;
			}

			IResourceManager resourceManager = GetResourceManager();
			if (resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumNameResourceKeyTemplate, _enumType.Name);
				return resourceManager.GetString(resourceName, cultureInfo);
			}

			return _enumType.Name;
		}

		/// <summary>
		/// Получает метаданные перечисления по ключу.
		/// </summary>
		/// <param name="metadataKey">Ключ метаданных.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Метаданные.</returns>
		public string GetEnumMetadata(string metadataKey, CultureInfo cultureInfo)
		{
			IResourceManager resourceManager = GetResourceManager();
			if (resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumMetadataResourceKeyTemplate, _enumType.Name, metadataKey);
				return resourceManager.GetString(resourceName, cultureInfo);
			}

			return null;
		}

		/// <summary>
		/// Получает локализованное название значения типа перечисления.
		/// </summary>
		/// <param name="enumValueName">Название значения перечисления.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное название значения типа перечисления.</returns>
		public string GetEnumValueName(string enumValueName, CultureInfo cultureInfo)
		{
			IResourceManager resourceManager = GetResourceManager();
			if (resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumValueNameResourceKeyTemplate, _enumType.Name, enumValueName);
				return resourceManager.GetString(resourceName, cultureInfo);
			}

			return enumValueName;
		}

		/// <summary>
		/// Получает метаданные для знчения перечисления по ключу.
		/// </summary>
		/// <param name="enumValueName">Название значения перечисления.</param>
		/// <param name="metadataKey">Ключ метаданных.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Метаданные.</returns>
		public string GetEnumValueMetadata(string enumValueName, string metadataKey, CultureInfo cultureInfo)
		{
			IResourceManager resourceManager = GetResourceManager();
			if (resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumValueMetadataResourceKeyTemplate, _enumType.Name, enumValueName, metadataKey);
				return resourceManager.GetString(resourceName, cultureInfo);
			}

			return null;
		}

		#endregion
	}
}