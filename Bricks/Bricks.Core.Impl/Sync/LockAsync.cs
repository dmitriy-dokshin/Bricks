#region

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Disposing;
using Bricks.Core.Seams;
using Bricks.Core.Sync;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.Impl.Sync
{
	/// <summary>
	/// The default implementation of <see cref="ILockAsync" />.
	/// </summary>
	internal sealed class LockAsync : ILockAsync
	{
		private readonly LockAsyncType _type;
		private int _currentCompletedTcsIndex;
		private IDisposableHelper _disposableHelper;
		private IInterlockedHelper _interlockedHelper;
		private Random _random;
		private IImmutableList<TaskCompletionSourceData> _taskCompletionSources;

		public LockAsync(LockAsyncType type = LockAsyncType.Random)
		{
			_taskCompletionSources = ImmutableList.Create<TaskCompletionSourceData>();
			_type = type;
			if (_type == LockAsyncType.Random)
			{
				_random = new Random();
			}

			_currentCompletedTcsIndex = -1;
		}

		[InjectionMethod]
		public void Initialize(IInterlockedHelper interlockedHelper, IDisposableHelper disposableHelper, IRandomProvider randomProvider)
		{
			_interlockedHelper = interlockedHelper;
			_disposableHelper = disposableHelper;
			if (_type == LockAsyncType.Random)
			{
				_random = randomProvider.Get();
			}
		}

		private void RemoveAndSetNextResult()
		{
			Tuple<int, TaskCompletionSourceData> tuple =
				_interlockedHelper.CompareExchange(
					ref _taskCompletionSources,
					x =>
						{
							TaskCompletionSourceData tcsData = x[_currentCompletedTcsIndex];
							IImmutableList<TaskCompletionSourceData> newValue = x.RemoveAt(_currentCompletedTcsIndex);
							int index = newValue.Count > 0 ? GetNextTcsIndex(newValue) : -1;
							return _interlockedHelper.CreateChangeResult(newValue, new Tuple<int, TaskCompletionSourceData>(index, tcsData));
						});
			tuple.Item2.Dispose();
			int tcsIndex = tuple.Item1;
			if (tcsIndex >= 0)
			{
				try
				{
					_currentCompletedTcsIndex = tcsIndex;
					_taskCompletionSources[_currentCompletedTcsIndex].Tsc.SetResult(_disposableHelper.Action(RemoveAndSetNextResult));
				}
				catch (InvalidOperationException)
				{
					// An exception could be thrown if task has been cancelled.
					// In that case it is necessary to remove the TaskCompletionSource object
					// and set result for next TaskCompletionSource object,
					// it is necessary i.e. repeat this function.
					RemoveAndSetNextResult();
				}
			}
		}

		private int GetNextTcsIndex(IImmutableList<TaskCompletionSourceData> taskCompletionSources)
		{
			int nextTcsIndex;
			switch (_type)
			{
				case LockAsyncType.Random:
					nextTcsIndex = _random.Next(taskCompletionSources.Count);
					break;
				case LockAsyncType.Queue:
					nextTcsIndex = 0;
					break;
				case LockAsyncType.Stack:
					nextTcsIndex = taskCompletionSources.Count - 1;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return nextTcsIndex;
		}

		private IChangeResult<IImmutableList<TaskCompletionSourceData>, TaskCompletionSourceData> AddTcsData(IImmutableList<TaskCompletionSourceData> x)
		{
			var tcs = new TaskCompletionSource<IDisposable>();
			if (x.Count == 0)
			{
				tcs.SetResult(_disposableHelper.Action(RemoveAndSetNextResult));
			}

			var tcsData = new TaskCompletionSourceData(tcs);
			return _interlockedHelper.CreateChangeResult(x.Add(tcsData), tcsData);
		}

		private IChangeResult<IImmutableList<TaskCompletionSourceData>, TaskCompletionSourceData> TryAddTcsData(IImmutableList<TaskCompletionSourceData> x)
		{
			TaskCompletionSourceData tcsData = null;
			IImmutableList<TaskCompletionSourceData> newValue;
			if (x.Count == 0)
			{
				var tcs = new TaskCompletionSource<IDisposable>();
				tcsData = new TaskCompletionSourceData(tcs);
				newValue = x.Add(tcsData);
			}
			else
			{
				newValue = x;
			}

			return _interlockedHelper.CreateChangeResult(newValue, tcsData);
		}

		private sealed class TaskCompletionSourceData : IDisposable
		{
			public TaskCompletionSourceData(TaskCompletionSource<IDisposable> tsc)
			{
				Tsc = tsc;
			}

			public TaskCompletionSource<IDisposable> Tsc { get; private set; }

			public CancellationTokenRegistration? CancellationTokenRegistration { get; set; }

			#region Implementation of IDisposable

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				if (CancellationTokenRegistration.HasValue)
				{
					CancellationTokenRegistration.Value.Dispose();
				}
			}

			#endregion
		}

		#region Implementation of ILockAsync

		/// <summary>
		/// Acquires the lock.
		/// </summary>
		/// <returns>An <see cref="IDisposable" /> object that is used to release the lock.</returns>
		public Task<IDisposable> Enter()
		{
			return Enter(CancellationToken.None);
		}

		/// <summary>
		/// Acquires the lock. Waiting for lock release could be cancelled through the <paramref name="cancellationToken" />.
		/// </summary>
		/// <param name="cancellationToken">
		/// The <see cref="CancellationToken" /> that is used to cancel waiting for lock release.
		/// </param>
		/// <returns>An <see cref="IDisposable" /> object that is used to release the lock.</returns>
		public Task<IDisposable> Enter(CancellationToken cancellationToken)
		{
			TaskCompletionSourceData tcsData =
				_interlockedHelper.CompareExchange(ref _taskCompletionSources, x => AddTcsData(x));
			if (tcsData.Tsc.Task.Status == TaskStatus.RanToCompletion)
			{
				_currentCompletedTcsIndex = 0;
			}

			if (cancellationToken != CancellationToken.None)
			{
				tcsData.CancellationTokenRegistration =
					cancellationToken.Register(() =>
						{
							try
							{
								tcsData.Tsc.SetCanceled();
							}
							catch (InvalidOperationException)
							{
							}
						});
			}

			return tcsData.Tsc.Task;
		}

		/// <summary>
		/// Attempts to acquire the lock.
		/// </summary>
		/// <param name="disposable">
		/// An <see cref="IDisposable" /> object that is used to release the lock. If the lock is acquired set <c>null</c>.
		/// </param>
		/// <returns>If the lock is not acquired returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public bool TryEnter(out IDisposable disposable)
		{
			TaskCompletionSourceData tcsData = _interlockedHelper.CompareExchange(ref _taskCompletionSources, x => TryAddTcsData(x));
			bool isEntered;
			if (tcsData != null)
			{
				disposable = _disposableHelper.Action(RemoveAndSetNextResult);
				tcsData.Tsc.SetResult(disposable);
				isEntered = true;
				_currentCompletedTcsIndex = 0;
			}
			else
			{
				disposable = null;
				isEntered = false;
			}

			return isEntered;
		}

		#endregion
	}
}