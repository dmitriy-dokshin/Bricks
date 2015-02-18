#region

using System;

#endregion

namespace Bricks.Core.Disposing
{
	/// <summary>
	/// Contains methods for the <see cref="IDisposable" />.
	/// </summary>
	public interface IDisposableHelper
	{
		/// <summary>
		/// Creates an <see cref="IDisposable" /> object that executes action <paramref name="dispose" /> when releasing resources.
		/// </summary>
		/// <param name="dispose">An <see cref="Action" /> that will be executed when releasing resources.</param>
		/// <returns>
		/// An <see cref="IDisposable" /> object that executes action <paramref name="dispose" /> when releasing resources.
		/// </returns>
		IDisposable Action(Action dispose);

		IDisposable GetEmpty();
	}
}