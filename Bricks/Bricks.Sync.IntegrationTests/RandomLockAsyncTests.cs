#region

using System;
using System.Collections.Generic;
using Bricks.Core.Disposing.Implementation;
using Bricks.Core.Seams;
using Bricks.Sync.Implementation;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace Bricks.Sync.IntegrationTests
{
	[TestFixture]
	internal sealed class RandomLockAsyncTests : TypedLockAsyncTestsBase
	{
		private const int Seed = 1;

		/// <summary>
		/// Gets an <see cref="ILockAsync" /> object for testing.
		/// </summary>
		/// <returns>An <see cref="ILockAsync" /> object for testing.</returns>
		protected override ILockAsync GetLockAsync()
		{
			var lockAsync = new LockAsync();
			var randomProvider = Substitute.For<IRandomProvider>();
			randomProvider.Get().Returns(new Random(Seed));
			lockAsync.Initialize(new InterlockedHelper(), new DisposableHelper(), randomProvider);
			return lockAsync;
		}

		protected override void CheckList(IReadOnlyList<int> list)
		{
			var temp = new List<int>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				temp.Add(i);
			}

			var expectedList = new List<int>(list.Count);
			var random = new Random(Seed);
			while (temp.Count > 0)
			{
				int index = random.Next(temp.Count);
				expectedList.Add(temp[index]);
				temp.RemoveAt(index);
			}

			for (int i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(expectedList[i], list[i]);
			}
		}
	}
}