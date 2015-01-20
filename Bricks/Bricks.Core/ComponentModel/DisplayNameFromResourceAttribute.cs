#region

using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

#endregion

namespace Bricks.Core.ComponentModel
{
	public sealed class DisplayNameFromResourceAttribute : Attribute
	{
		public DisplayNameFromResourceAttribute(Type resourceType, string resourceName)
		{
			ResourceType = resourceType;
			ResourceName = resourceName;
		}

		public Type ResourceType { get; private set; }

		public string ResourceName { get; private set; }

		public string GetName(CultureInfo cultureInfo = null)
		{
			var propertyInfo = ResourceType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic);
			var resourceManager = (ResourceManager)propertyInfo.GetValue(null);
			return resourceManager.GetString(ResourceName, cultureInfo ?? CultureInfo.CurrentCulture);
		}
	}
}