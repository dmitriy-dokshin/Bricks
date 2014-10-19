#region

using System;

#endregion

namespace Bricks.Core.Disposing.Implementation
{
	/// <summary>
	/// Базовый класс для реализации <see cref="IDisposable" />.
	/// </summary>
	public abstract class DisposableBase : IDisposable
	{
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
		/// Признак того, что ресурсы уже освобождены.
		/// </summary>
		protected bool IsDisposed { get; private set; }

		/// <summary>
		/// Освобождает ресурсы, связанные с объектом.
		/// </summary>
		/// <param name="disposing">
		/// Признак освобождения ресурсов при вызове метода <see cref="IDisposable.Dispose" /> (если
		/// <c>true</c>) или при вызове финализатор (если <c>false</c>).
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