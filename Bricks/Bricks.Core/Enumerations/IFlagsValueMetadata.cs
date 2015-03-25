#region

using System.Collections.Generic;

#endregion

namespace Bricks.Core.Enumerations
{
	/// <summary>
	/// Интерфейс метаданных значения флагового перечисления.
	/// </summary>
	public interface IFlagsValueMetadata : IEnumValueMetadata
	{
		/// <summary>
		/// Метаданные флагов значения перечисления.
		/// </summary>
		IReadOnlyCollection<IFlagsValueMetadata> FlagsValueMetadatas { get; }
	}
}