#region

using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

#endregion

namespace Bricks.Sync.Tests
{
	/// <summary>
	/// The base class for the <see cref="ILockAsync" /> testing.
	/// </summary>
	public abstract class LockAsyncTestsBase
	{
		/// <summary>
		/// Gets an <see cref="ILockAsync" /> object for testing.
		/// </summary>
		/// <returns>An <see cref="ILockAsync" /> object for testing.</returns>
		protected abstract ILockAsync GetLockAsync();

		[Test]
		public void TryEnter_LockIsNotEntered_ReturnsTrue()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			bool isEntered = lockAsync.TryEnter(out disposable);

			// Arrange
			Assert.IsTrue(isEntered);
		}

		[Test]
		public void TryEnter_LockIsNotEntered_DisposableIsNotNull()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);

			// Arrange
			Assert.IsNotNull(disposable);
		}

		[Test]
		public void TryEnter_LockIsEntered_ReturnsFalse()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			bool isEntered = lockAsync.TryEnter(out disposable);

			// Arrange
			Assert.IsFalse(isEntered);
		}

		[Test]
		public void TryEnter_LockIsEntered_DisposableIsNull()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			lockAsync.TryEnter(out disposable);

			// Arrange
			Assert.IsNull(disposable);
		}

		[Test]
		public void TryEnter_AfterDisposing_ReturnsTrue()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			disposable.Dispose();
			bool isEntered = lockAsync.TryEnter(out disposable);

			// Arrange
			Assert.IsTrue(isEntered);
		}

		[Test]
		public void TryEnter_AfterDisposing_DisposableIsNotNull()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			disposable.Dispose();
			lockAsync.TryEnter(out disposable);

			// Arrange
			Assert.IsNotNull(disposable);
		}

		[Test]
		public void Enter_LockIsEntered_TaskIsInWaitingForActivationStatus()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			lockAsync.Enter();
			Task task = lockAsync.Enter();

			// Arrange
			Assert.AreEqual(TaskStatus.WaitingForActivation, task.Status);
		}

		[Test]
		public async Task Enter_AfterDisposing_TaskIsInRanToCompletionStatus()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable = await lockAsync.Enter();
			Task task = lockAsync.Enter();
			disposable.Dispose();

			// Arrange
			Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
		}

		[Test]
		public void Enter_LockIsNotEntered_TaskIsInRanToCompletionStatus()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			Task task = lockAsync.Enter();

			// Arrange
			Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
		}

		[Test]
		public void EnterWithCancellationTokenAndCancel_LockIsEntered_TaskIsInCanceledStatus()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			lockAsync.Enter();
			var cancellationTokenSource = new CancellationTokenSource();
			Task task = lockAsync.Enter(cancellationTokenSource.Token);
			cancellationTokenSource.Cancel();

			// Arrange
			Assert.AreEqual(TaskStatus.Canceled, task.Status);
		}

		[Test]
		public void EnterWithCancellationTokenAndCancel_LockIsNotEntered_TaskIsInRanToCompletionStatus()
		{
			// Assert
			ILockAsync lockAsync = GetLockAsync();

			// Act
			var cancellationTokenSource = new CancellationTokenSource();
			Task task = lockAsync.Enter(cancellationTokenSource.Token);
			cancellationTokenSource.Cancel();

			// Arrange
			Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
		}
	}
}