#region

using System.Collections.Generic;
using System.Threading.Tasks;

using Bricks.Core.Tests.Sync;

using NUnit.Framework;

#endregion

namespace Bricks.Core.Impl.IntegrationTests.Sync
{
	public abstract class TypedLockAsyncTestsBase : LockAsyncTestsBase
	{
		[TestCase(10)]
		public async Task AddToList_Syncroniosly_OrderOfItemsIsCorrect(int count)
		{
			var lockAsync = GetLockAsync();

			var list = new List<int>();
			var tasks = new List<Task>();
			using (await lockAsync.Enter())
			{
				for (var i = 0; i < count; i++)
				{
					var i1 = i;
					Task task = lockAsync.Enter().ContinueWith(async x =>
						{
							using (await x)
							{
								list.Add(i1);
							}
						});
					tasks.Add(task);
				}
			}

			await Task.WhenAll(tasks.ToArray());

			CheckList(list);
		}

		protected abstract void CheckList(IReadOnlyList<int> list);
	}
}