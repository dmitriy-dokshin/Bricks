#region

using System.Collections.Generic;
using System.Linq;

using Bricks.Core.Enum;

#endregion

namespace Bricks.Core.Impl.Enum
{
	/// <summary>
	/// Метаданные значения флагового перечисления, полученные из ресурсов.
	/// </summary>
	internal sealed class ResourceFlagsValueMetadata : ResourceEnumValueMetadata, IFlagsValueMetadata
	{
		public ResourceFlagsValueMetadata(IFlagsMetadata flagsMetadata, IEnumResourceHelper enumResourceHelper, System.Enum enumValue)
			: base(flagsMetadata, enumResourceHelper, enumValue)
		{
			if (!flagsMetadata.Flags.Contains(enumValue))
			{
				var enumValueFlags = flagsMetadata.Flags.Where(enumValue.HasFlag);
				FlagsValueMetadatas = enumValueFlags.Select(flagsMetadata.GetFlagsValueMetadata).ToArray();
			}
		}

		#region Implementation of IFlagsValueMetadata

		/// <summary>
		/// Метаданные флагов значения перечисления.
		/// </summary>
		public IReadOnlyCollection<IFlagsValueMetadata> FlagsValueMetadatas { get; private set; }

		#endregion
	}
}