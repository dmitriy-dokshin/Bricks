using System;

namespace Bricks.Core.IO
{
	public interface IFileSystemWatcher : IDisposable
	{
		IDisposable Add(string path, string filter = null, bool includeSubdirectories = false);

		void Remove(string path, string filter = null);
	}
}