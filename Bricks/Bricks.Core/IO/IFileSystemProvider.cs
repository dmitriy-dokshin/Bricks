#region

using System.IO;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.IO
{
	public interface IFileSystemProvider
	{
		IResult<Stream> Open(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None);
	}
}