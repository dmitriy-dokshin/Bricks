#region

using System.IO;

using Bricks.Core.Environment;
using Bricks.Core.Exceptions;
using Bricks.Core.IO;
using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Impl.IO
{
	public sealed class FileSystemProvider : IFileSystemProvider
	{
		private readonly IEnvironment _environment;
		private readonly IExceptionHelper _exceptionHelper;

		public FileSystemProvider(IExceptionHelper exceptionHelper, IEnvironment environment)
		{
			_exceptionHelper = exceptionHelper;
			_environment = environment;
		}

		#region Implementation of IFileSystemProvider

		public IResult<Stream> Open(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None)
		{
			if (!Path.IsPathRooted(path))
			{
				path = Path.Combine(_environment.RootPath, path);
			}

			return _exceptionHelper.Catch(() => File.Open(path, mode, access, share));
		}

		#endregion
	}
}