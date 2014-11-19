#region

using System;

#endregion

namespace Bricks.WebAPI.Filters
{
	/// <summary>
	/// Атрибут, указывающий, что параметр не может быть равен <c>null</c>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class NotNullAttribute : Attribute
	{
	}
}