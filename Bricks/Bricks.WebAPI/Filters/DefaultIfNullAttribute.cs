#region

using System;

#endregion

namespace Bricks.WebAPI.Filters
{
	/// <summary>
	/// Атрибут, указывающий, что нужно создать объект по умолчанию, если параметр равен <c>null</c>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class DefaultIfNullAttribute : Attribute
	{
	}
}