using System.Reflection;

namespace Bricks.Helpers.Reflection
{
	public interface IReflectionHelper
	{
		string GetFullName(MethodBase method);
	}
}