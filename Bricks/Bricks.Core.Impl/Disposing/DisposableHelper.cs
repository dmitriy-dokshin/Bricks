#region

using System;

using Bricks.Core.Disposing;

#endregion

namespace Bricks.Core.Impl.Disposing
{
	/// <summary>
	/// The default implementation of <see cref="IDisposableHelper" />.
	/// </summary>
	internal sealed class DisposableHelper : IDisposableHelper
	{
		#region Implementation of IDisposableHelper

		/// <summary>
		/// Creates an <see cref="IDisposable" /> object that executes action <paramref name="dispose" /> when releasing resources.
		/// </summary>
		/// <param name="dispose">An <see cref="Action" /> that will be executed when releasing resources.</param>
		/// <returns>
		/// An <see cref="IDisposable" /> object that executes action <paramref name="dispose" /> when releasing resources.
		/// </returns>
		public IDisposable Action(Action dispose)
		{
			return new ActionDisposable(dispose);
		}

		#endregion
	}
}