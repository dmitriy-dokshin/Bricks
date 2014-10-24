#region

using System;

#endregion

namespace Bricks.Core.Seams.Implementation
{
	/// <summary>
	/// The default implementation of <see cref="IRandomProvider" />.
	/// </summary>
	internal sealed class RandomProvider : IRandomProvider
	{
		#region Implementation of IRandomProvider

		/// <summary>
		/// Gets a <see cref="Random" /> object.
		/// </summary>
		/// <returns>A <see cref="Random" /> object.</returns>
		public Random Get()
		{
			return new Random();
		}

		#endregion
	}
}