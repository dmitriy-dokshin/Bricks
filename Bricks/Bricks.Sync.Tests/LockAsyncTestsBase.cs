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
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			bool isEntered = lockAsync.TryEnter(out disposable);

			// Assert
			Assert.IsTrue(isEntered);
		}

		[Test]
		public void TryEnter_LockIsNotEntered_DisposableIsNotNull()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);

			// Assert
			Assert.IsNotNull(disposable);
		}

		[Test]
		public void TryEnter_LockIsEntered_ReturnsFalse()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			bool isEntered = lockAsync.TryEnter(out disposable);

			// Assert
			Assert.IsFalse(isEntered);
		}

		[Test]
		public void TryEnter_LockIsEntered_DisposableIsNull()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			lockAsync.TryEnter(out disposable);

			// Assert
			Assert.IsNull(disposable);
		}

		[Test]
		public void TryEnter_AfterDisposing_ReturnsTrue()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			disposable.Dispose();
			bool isEntered = lockAsync.TryEnter(out disposable);

			// Assert
			Assert.IsTrue(isEntered);
		}

		[Test]
		public void TryEnter_AfterDisposing_DisposableIsNotNull()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable;
			lockAsync.TryEnter(out disposable);
			disposable.Dispose();
			lockAsync.TryEnter(out disposable);

			// Assert
			Assert.IsNotNull(disposable);
		}

		[Test]
		public void Enter_LockIsEntered_TaskIsInWaitingForActivationStatus()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			lockAsync.Enter();
			Task task = lockAsync.Enter();

			// Assert
			Assert.AreEqual(TaskStatus.WaitingForActivation, task.Status);
		}

		[Test]
		public async Task Enter_AfterDisposing_TaskIsInRanToCompletionStatus()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			IDisposable disposable = await lockAsync.Enter();
			Task task = lockAsync.Enter();
			disposable.Dispose();

			// Assert
			Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
		}

		[Test]
		public void Enter_LockIsNotEntered_TaskIsInRanToCompletionStatus()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			Task task = lockAsync.Enter();

			// Assert
			Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
		}

		[Test]
		public void EnterWithCancellationTokenAndCancel_LockIsEntered_TaskIsInCanceledStatus()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			lockAsync.Enter();
			var cancellationTokenSource = new CancellationTokenSource();
			Task task = lockAsync.Enter(cancellationTokenSource.Token);
			cancellationTokenSource.Cancel();

			// Assert
			Assert.AreEqual(TaskStatus.Canceled, task.Status);
		}

		[Test]
		public void EnterWithCancellationTokenAndCancel_LockIsNotEntered_TaskIsInRanToCompletionStatus()
		{
			// Arrange
			ILockAsync lockAsync = GetLockAsync();

			// Act
			var cancellationTokenSource = new CancellationTokenSource();
			Task task = lockAsync.Enter(cancellationTokenSource.Token);
			cancellationTokenSource.Cancel();

			// Assert
			Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
		}
	}
}