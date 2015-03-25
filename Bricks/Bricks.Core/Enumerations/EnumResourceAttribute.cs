#region

using System;

#endregion

namespace Bricks.Core.Enumerations
{
	/// <summary>
	/// Аттрибут, используемый для указания ресурса перечисления.
	/// </summary>
	[AttributeUsage(AttributeTargets.Enum)]
	public sealed class EnumResourceAttribute : Attribute
	{
		/// <summary>
		/// Имя встроенного ресурса.
		/// </summary>
		public string BaseName { get; set; }

		/// <summary>
		/// Имя сборки.
		/// </summary>
		public string AssemblyString { get; set; }

		/// <summary>
		/// Тип ресурса.
		/// </summary>
		public Type ResourceType { get; set; }

		/// <summary>
		/// Полное имя типа ресурса.
		/// </summary>
		public string ResourceTypeFullName { get; set; }
	}
}