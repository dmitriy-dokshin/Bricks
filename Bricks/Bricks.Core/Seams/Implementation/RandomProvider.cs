#region

using System;

#endregion

namespace Bricks.Core.Seams.Implementation
{
	internal sealed class RandomProvider : IRandomProvider
	{
		#region Implementation of IRandomProvider

		public Random Get()
		{
			return new Random();
		}

		#endregion
	}
}