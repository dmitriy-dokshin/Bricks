#region

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

using Bricks.Core.Sync;
using Bricks.Core.Tasks;

#endregion

namespace Bricks.Core.Impl.Tasks
{
	internal sealed class TaskHelper : ITaskHelper
	{
		private readonly Task _emptyTask;
		private readonly IInterlockedHelper _interlockedHelper;
		private IImmutableDictionary<Type, Task> _emptyTasks;

		public TaskHelper(IInterlockedHelper interlockedHelper)
		{
			_interlockedHelper = interlockedHelper;
			_emptyTask = Task.FromResult((object)null);
			_emptyTasks = ImmutableDictionary.Create<Type, Task>();
		}

		#region Implementation of ITaskHelper

		public Task GetEmpty()
		{
			return _emptyTask;
		}

		public Task<TResult> GetEmpty<TResult>()
		{
			Type type = typeof(TResult);
			return (Task<TResult>)_interlockedHelper.CompareExchange(ref _emptyTasks, x =>
				{
					Task task;
					IImmutableDictionary<Type, Task> newValue;
					if (!x.TryGetValue(type, out task))
					{
						task = Task.FromResult(default(TResult));
						newValue = x.Add(type, task);
					}
					else
					{
						newValue = x;
					}

					return _interlockedHelper.CreateChangeResult(newValue, task);
				});
		}

		#endregion
	}
}