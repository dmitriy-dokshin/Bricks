#region

using System;

#endregion

namespace Bricks.Core.Seams
{
	/// <summary>
	/// Represents a provider for <see cref="Random" /> objects.
	/// </summary>
	public interface IRandomProvider
	{
		/// <summary>
		/// Gets a <see cref="Random" /> object.
		/// </summary>
		/// <returns>A <see cref="Random" /> object.</returns>
		Random Get();
	}
}