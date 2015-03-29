#region

using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;

using Bricks.Core.Disposing;
using Bricks.Core.Environment;
using Bricks.Core.Events;
using Bricks.Core.Exceptions;
using Bricks.Core.IO;
using Bricks.Core.Sync;

#endregion

namespace Bricks.Core.Impl.IO
{
	public sealed class FileSystemWatcherImpl : IFileSystemWatcher
	{
		private readonly IDisposableHelper _disposableHelper;
		private readonly IEnvironment _environment;
		private readonly IEventManager _eventManager;
		private readonly IExceptionHelper _exceptionHelper;
		private readonly IInterlockedHelper _interlockedHelper;
		private IImmutableDictionary<FileSystemWatcherParameters, FileSystemWatcher> _fileSystemWatchers;

		public FileSystemWatcherImpl(IEnvironment environment, IEventManager eventManager, IExceptionHelper exceptionHelper, IInterlockedHelper interlockedHelper, IDisposableHelper disposableHelper)
		{
			_environment = environment;
			_eventManager = eventManager;
			_exceptionHelper = exceptionHelper;
			_interlockedHelper = interlockedHelper;
			_disposableHelper = disposableHelper;
			_fileSystemWatchers = ImmutableDictionary.Create<FileSystemWatcherParameters, FileSystemWatcher>();
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			foreach (var fileSystemWatcher in _fileSystemWatchers.Values)
			{
				fileSystemWatcher.Dispose();
			}
		}

		#endregion

		private sealed class FileSystemWatcherParameters
		{
			public FileSystemWatcherParameters(string path, string filter, bool includeSubdirectories = false)
			{
				Path = path;
				Filter = filter;
				IncludeSubdirectories = includeSubdirectories;
			}

			public string Filter { get; private set; }

			public string Path { get; private set; }

			public bool IncludeSubdirectories { get; private set; }

			public override bool Equals(object obj)
			{
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}

				var parameters = (FileSystemWatcherParameters)obj;
				bool @equals =
					string.Equals(Path, parameters.Path)
					&& string.Equals(Filter, parameters.Filter);
				return @equals;
			}

			public override int GetHashCode()
			{
				int hashCode = Path.GetHashCode();
				if (!string.IsNullOrEmpty(Filter))
				{
					hashCode |= Filter.GetHashCode();
				}

				return hashCode;
			}
		}

		#region Implementation of IFileSystemWatcher

		public IDisposable Add(string path, string filter = null, bool includeSubdirectories = false)
		{
			if (!Path.IsPathRooted(path))
			{
				path = _environment.RootPath + path;
			}

			var parameters = new FileSystemWatcherParameters(path, filter, includeSubdirectories);
			Tuple<FileSystemWatcher, bool> result = _interlockedHelper.CompareExchange(ref _fileSystemWatchers, x => GetOrAddFileSystemWatcher(x, parameters));
			FileSystemWatcher fileSystemWatcher = result.Item1;
			bool isNew = result.Item2;
			if (isNew)
			{
				fileSystemWatcher.EnableRaisingEvents = true;
			}

			IDisposable disposable = _disposableHelper.Action(() => Remove(path, filter));
			return disposable;
		}

		private IChangeResult<IImmutableDictionary<FileSystemWatcherParameters, FileSystemWatcher>, Tuple<FileSystemWatcher, bool>> GetOrAddFileSystemWatcher(IImmutableDictionary<FileSystemWatcherParameters, FileSystemWatcher> x, FileSystemWatcherParameters parameters)
		{
			FileSystemWatcher fileSystemWatcher;
			IImmutableDictionary<FileSystemWatcherParameters, FileSystemWatcher> newValue;
			if (!x.TryGetValue(parameters, out fileSystemWatcher))
			{
				fileSystemWatcher = string.IsNullOrEmpty(parameters.Filter) ? new FileSystemWatcher(parameters.Path) : new FileSystemWatcher(parameters.Path, parameters.Filter);
				fileSystemWatcher.IncludeSubdirectories = parameters.IncludeSubdirectories;
				FileSystemEventHandler crudHandler = (sender, args) => _exceptionHelper.SimpleCatchAsync(_eventManager.Raise(sender, args, CancellationToken.None));
				fileSystemWatcher.Created += crudHandler;
				fileSystemWatcher.Changed += crudHandler;
				fileSystemWatcher.Deleted += crudHandler;
				fileSystemWatcher.Renamed += (sender, args) => _exceptionHelper.SimpleCatchAsync(_eventManager.Raise(sender, args, CancellationToken.None));
				newValue = x.Add(parameters, fileSystemWatcher);
			}
			else
			{
				newValue = x;
			}

			var result = new Tuple<FileSystemWatcher, bool>(fileSystemWatcher, x != newValue);
			return _interlockedHelper.CreateChangeResult(newValue, result);
		}

		public void Remove(string path, string filter = null)
		{
			if (!Path.IsPathRooted(path))
			{
				path = _environment.RootPath + path;
			}

			var key = new FileSystemWatcherParameters(path, filter);
			FileSystemWatcher fileSystemWatcher = _interlockedHelper.CompareExchange(ref _fileSystemWatchers, x => TryRemoveFileSystemWatcher(x, key));
			if (fileSystemWatcher != null)
			{
				fileSystemWatcher.Dispose();
			}
		}

		private IChangeResult<IImmutableDictionary<FileSystemWatcherParameters, FileSystemWatcher>, FileSystemWatcher> TryRemoveFileSystemWatcher(IImmutableDictionary<FileSystemWatcherParameters, FileSystemWatcher> x, FileSystemWatcherParameters parameters)
		{
			FileSystemWatcher fileSystemWatcher;
			IImmutableDictionary<FileSystemWatcherParameters, FileSystemWatcher> newValue =
				x.TryGetValue(parameters, out fileSystemWatcher) ? x.Remove(parameters) : x;
			return _interlockedHelper.CreateChangeResult(newValue, fileSystemWatcher);
		}

		#endregion
	}
}