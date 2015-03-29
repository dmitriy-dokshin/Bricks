#region

using System.IO;

#endregion

namespace Bricks.Core.Environment
{
	public static class EnvironmentExtensions
	{
		public static string GetRootedPath(this IEnvironment environment, string path)
		{
			string rootedPath = Path.IsPathRooted(path) ? path : Path.Combine(environment.RootPath, path);
			return rootedPath;
		}
	}
}