#region

using System.Collections.Generic;
using System.Threading.Tasks;
using Bricks.Sync.Tests;
using NUnit.Framework;

#endregion

namespace Bricks.Sync.IntegrationTests
{
	public abstract class TypedLockAsyncTestsBase : LockAsyncTestsBase
	{
		[TestCase(10)]
		public void AddToList_Syncroniosly_OrderOfItemsIsCorrect(int count)
		{
			ILockAsync lockAsync = GetLockAsync();

			var list = new List<int>();
			var tasks = new List<Task>();
			using (lockAsync.Enter().Result)
			{
				for (int i = 0; i < count; i++)
				{
					int i1 = i;
					Task task = lockAsync.Enter().ContinueWith(x =>
					{
						using (x.Result)
						{
							list.Add(i1);
						}
					});
					tasks.Add(task);
				}
			}

			Task.WaitAll(tasks.ToArray());

			CheckList(list);
		}

		protected abstract void CheckList(IReadOnlyList<int> list);
	}
}