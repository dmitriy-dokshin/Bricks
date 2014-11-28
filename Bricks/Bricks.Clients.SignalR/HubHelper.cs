#region

using System;

using Microsoft.AspNet.SignalR.Hubs;

#endregion

namespace Bricks.Clients.SignalR
{
	internal static class HubHelper
	{
		internal static string GetHubName(Type type)
		{
			if (!typeof(IHub).IsAssignableFrom(type))
			{
				return null;
			}
			return GetHubAttributeName(type) ?? GetHubTypeName(type);
		}

		internal static string GetHubAttributeName(Type type)
		{
			if (!typeof(IHub).IsAssignableFrom(type))
			{
				return null;
			}

			return ReflectionHelper.GetAttributeValue(type, (Func<HubNameAttribute, string>)(attr => attr.HubName));
		}

		private static string GetHubTypeName(Type type)
		{
			int genericIndex = type.Name.LastIndexOf('`');
			if (genericIndex == -1)
			{
				return type.Name;
			}

			return type.Name.Substring(0, genericIndex);
		}
	}
}