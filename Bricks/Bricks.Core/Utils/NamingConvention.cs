using System;

namespace Bricks.Core.Utils
{
	[Flags]
	public enum NamingConvention
	{
		CamelCase = 1,
		LowerCamelCase = 2,
		UpperCase = 4,
		LowerCase = 8
	}
}