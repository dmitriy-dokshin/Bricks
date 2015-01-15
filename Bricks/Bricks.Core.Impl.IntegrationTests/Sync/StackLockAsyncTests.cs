#region

using System.Collections.Generic;

using Bricks.Core.Impl.Disposing;
using Bricks.Core.Impl.Sync;
using Bricks.Core.Sync;

using NUnit.Framework;

#endregion

namespace Bricks.Core.Impl.IntegrationTests.Sync
{
	[TestFixture]
	internal sealed class StackLockAsyncTests : TypedLockAsyncTestsBase
	{
		/// <summary>
		/// Gets an <see cref="ILockAsync" /> object for testing.
		/// </summary>
		/// <returns>An <see cref="ILockAsync" /> object for testing.</returns>
		protected override ILockAsync GetLockAsync()
		{
			var lockAsync = new LockAsync(LockAsyncType.Stack);
			lockAsync.Initialize(new InterlockedHelper(), new DisposableHelper(), null);
			return lockAsync;
		}

		protected override void CheckList(IReadOnlyList<int> list)
		{
			for (var i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(i, list[list.Count - (i + 1)]);
			}
		}
	}
}