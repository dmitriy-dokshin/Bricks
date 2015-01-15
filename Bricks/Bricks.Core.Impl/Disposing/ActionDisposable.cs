#region

using System;

using Bricks.Core.Disposing;

#endregion

namespace Bricks.Core.Impl.Disposing
{
	/// <summary>
	/// The implementation of the <see cref="IDisposable" /> that excutes delegate when releasing resources.
	/// </summary>
	internal sealed class ActionDisposable : DisposableBase
	{
		private readonly Action _dispose;

		public ActionDisposable(Action dispose)
		{
			_dispose = dispose;
		}

		#region Overrides of DisposableBase

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">
		/// <c>true</c> if method is called from <see cref="Dispose" />; <c>false</c> if method is called by finalizer.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (IsDisposed)
			{
				return;
			}

			if (disposing)
			{
				_dispose();
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}