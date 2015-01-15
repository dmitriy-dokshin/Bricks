#region

using System.Reflection;

#endregion

namespace Bricks.Core.Reflection
{
	public interface IReflectionHelper
	{
		string GetFullName(MethodBase method);
	}
}