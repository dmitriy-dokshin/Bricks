#region

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bricks.Core.Disposing;
using Bricks.Core.Seams;
using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Sync.Implementation
{
	/// <summary>
	/// The default implementation of <see cref="ILockAsync" />.
	/// </summary>
	internal sealed class LockAsync : ILockAsync
	{
		private Random _random;
		private readonly LockAsyncType _type;
		private int _currentCompletedTcsIndex;
		private IDisposableHelper _disposableHelper;
		private IInterlockedHelper _interlockedHelper;
		private IImmutableList<TaskCompletionSource<IDisposable>> _taskCompletionSources;

		public LockAsync(LockAsyncType type = LockAsyncType.Random)
		{
			_taskCompletionSources = ImmutableList.Create<TaskCompletionSource<IDisposable>>();
			_type = type;
			if (_type == LockAsyncType.Random)
			{
				_random = new Random();
			}
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
			int? tcsIndex =
				_interlockedHelper.CompareExchange(ref _taskCompletionSources,
					x =>
					{
						IImmutableList<TaskCompletionSource<IDisposable>> newValue = x.RemoveAt(_currentCompletedTcsIndex);
						return _interlockedHelper.CreateChangeResult(newValue, newValue.Count > 0 ? GetNextTcsIndex(newValue) : (int?) null);
					});
			if (tcsIndex.HasValue)
			{
				try
				{
					_currentCompletedTcsIndex = tcsIndex.Value;
					_taskCompletionSources[_currentCompletedTcsIndex].SetResult(_disposableHelper.Action(RemoveAndSetNextResult));
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

		private int GetNextTcsIndex(IImmutableList<TaskCompletionSource<IDisposable>> taskCompletionSources)
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
			TaskCompletionSource<IDisposable> tcs1 =
				_interlockedHelper.CompareExchange(ref _taskCompletionSources,
					x =>
					{
						var tcs = new TaskCompletionSource<IDisposable>();
						if (x.Count == 0)
						{
							tcs.SetResult(_disposableHelper.Action(RemoveAndSetNextResult));
						}

						return _interlockedHelper.CreateChangeResult(x.Add(tcs), tcs);
					});
			if (tcs1.Task.Status == TaskStatus.RanToCompletion)
			{
				_currentCompletedTcsIndex = 0;
			}

			if (cancellationToken != CancellationToken.None)
			{
				cancellationToken.Register(() =>
				{
					try
					{
						tcs1.SetCanceled();
					}
					catch (InvalidOperationException)
					{
					}
				});
			}

			return tcs1.Task;
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
			TaskCompletionSource<IDisposable> tcs1 =
				_interlockedHelper.CompareExchange(ref _taskCompletionSources,
					x =>
					{
						TaskCompletionSource<IDisposable> tcs = null;
						IImmutableList<TaskCompletionSource<IDisposable>> newValue;
						if (x.Count == 0)
						{
							tcs = new TaskCompletionSource<IDisposable>();
							newValue = x.Add(tcs);
						}
						else
						{
							newValue = x;
						}

						return _interlockedHelper.CreateChangeResult(newValue, tcs);
					});

			bool isEntered;
			if (tcs1 != null)
			{
				disposable = _disposableHelper.Action(RemoveAndSetNextResult);
				tcs1.SetResult(disposable);
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