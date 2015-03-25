#region

using System;

#endregion

namespace Bricks.Core.Extensions
{
	public static class ReflectionExtensions
	{
		public static bool IsNullable(this Type type)
		{
			return !type.IsValueType || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
	}
}