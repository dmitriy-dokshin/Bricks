#region

using System;

#endregion

namespace Bricks.Core.Seams
{
	public interface IRandomProvider
	{
		Random Get();
	}
}