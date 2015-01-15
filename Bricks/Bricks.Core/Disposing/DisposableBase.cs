#region

using System;

#endregion

namespace Bricks.Core.Disposing
{
	/// <summary>
	/// The base class for the <see cref="IDisposable" /> implementation.
	/// </summary>
	public abstract class DisposableBase : IDisposable
	{
		/// <summary>
		/// <c>true</c> if resources have been already released.
		/// </summary>
		protected bool IsDisposed { get; private set; }

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">
		/// <c>true</c> if method is called from <see cref="Dispose" />; <c>false</c> if method is called by finalizer.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			IsDisposed = true;
		}

		~DisposableBase()
		{
			Dispose(false);
		}
	}
}