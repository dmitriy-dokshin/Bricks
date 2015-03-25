#region

using System;
using System.Collections.Generic;
using System.Linq;

using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	/// <summary>
	/// Метаданные значения флагового перечисления, полученные из ресурсов.
	/// </summary>
	internal sealed class ResourceFlagsValueMetadata : ResourceEnumValueMetadata, IFlagsValueMetadata
	{
		public ResourceFlagsValueMetadata(IFlagsMetadata flagsMetadata, IEnumResourceHelper enumResourceHelper, Enum enumValue)
			: base(flagsMetadata, enumResourceHelper, enumValue)
		{
			if (!flagsMetadata.Flags.Contains(enumValue))
			{
				IEnumerable<Enum> enumValueFlags = flagsMetadata.Flags.Where(enumValue.HasFlag);
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