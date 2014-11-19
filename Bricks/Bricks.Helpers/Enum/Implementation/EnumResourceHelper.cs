#region

using System;
using System.Globalization;
using System.Reflection;

using Bricks.Core.Resources;

#endregion

namespace Bricks.Helpers.Enum.Implementation
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
		/// Шаблон ключа описания перечисления в ресурсе.
		/// <para>{0}: название типа перечисления.</para>
		/// </summary>
		private const string EnumDescriptionResourceKeyTemplate = "{0}_Description";

		/// <summary>
		/// Шаблон ключа названия значения перечисления в ресурсе.
		/// <para>{0}: название типа перечисления.</para>
		/// <para>{1}: название значения перечисления.</para>
		/// </summary>
		private const string EnumValueNameResourceKeyTemplate = "{0}_{1}";

		/// <summary>
		/// Шаблон ключа описания значения перечисления в ресурсе.
		/// <para>{0}: название типа перечисления.</para>
		/// <para>{1}: название значения перечисления.</para>
		/// </summary>
		private const string EnumValueDescriptionResourceKeyTemplate = "{0}_{1}_Description";

		private readonly Type _enumType;
		private readonly IResourceManager _resourceManager;

		public EnumResourceHelper(Type enumType, IResourceProvider resourceProvider)
		{
			_enumType = enumType;

			var enumResourceAttribute =
			enumType.GetCustomAttribute<EnumResourceAttribute>(false);
			if (enumResourceAttribute != null)
			{
				if (!string.IsNullOrEmpty(enumResourceAttribute.BaseName))
				{
					Assembly resourceAssembly =
					!string.IsNullOrEmpty(enumResourceAttribute.AssemblyString)
					? Assembly.Load(enumResourceAttribute.AssemblyString)
					: enumType.Assembly;
					_resourceManager = resourceProvider.GetResourceManager(enumResourceAttribute.BaseName, resourceAssembly);
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
						_resourceManager = resourceProvider.GetResourceManager(resourceType);
					}
				}
			}
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

			if (_resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumNameResourceKeyTemplate, _enumType.Name);
				return _resourceManager.GetString(resourceName, cultureInfo);
			}

			return _enumType.Name;
		}

		/// <summary>
		/// Получает локализованное описание типа перечисления.
		/// </summary>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное описание типа перечисления.</returns>
		public string GetEnumDescription(CultureInfo cultureInfo)
		{
			if (_resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumDescriptionResourceKeyTemplate, _enumType.Name);
				return _resourceManager.GetString(resourceName, cultureInfo);
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
			if (_resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumValueNameResourceKeyTemplate, _enumType.Name, enumValueName);
				return _resourceManager.GetString(resourceName, cultureInfo);
			}

			return enumValueName;
		}

		/// <summary>
		/// Получает локализованное описание значения типа перечисления.
		/// </summary>
		/// <param name="enumValueName">Название значения перечисления.</param>
		/// <param name="cultureInfo">Информация о культуре.</param>
		/// <returns>Локализованное описание значения типа перечисления.</returns>
		public string GetEnumValueDescription(string enumValueName, CultureInfo cultureInfo)
		{
			if (_resourceManager != null)
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, EnumValueDescriptionResourceKeyTemplate, _enumType.Name, enumValueName);
				return _resourceManager.GetString(resourceName, cultureInfo);
			}

			return null;
		}

		#endregion
	}
}