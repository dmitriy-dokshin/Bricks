#region

using System;

#endregion

namespace Bricks.Core.Disposing.Implementation
{
	/// <summary>
	/// Представляет реализацию <see cref="IDisposable" />, в которой при освобождении ресурсов вызывается делегат.
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
		/// Освобождает ресурсы, связанные с объектом.
		/// </summary>
		/// <param name="disposing">
		/// Признак освобождения ресурсов при вызове метода <see cref="IDisposable.Dispose" /> (если
		/// <c>true</c>) или при вызове финализатор (если <c>false</c>).
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