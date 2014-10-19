#region

using System;

#endregion

namespace Bricks.Core.Disposing.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IDisposableHelper" />.
	/// </summary>
	internal sealed class DisposableHelper : IDisposableHelper
	{
		#region Implementation of IDisposableHelper

		/// <summary>
		/// Создаёт объект <see cref="IDisposable" />, выполняющий действие <paramref name="dispose" /> при освобождении
		/// ресурсов.
		/// </summary>
		/// <param name="dispose">Действие, выполняемое при освобождении ресурсов.</param>
		/// <returns>
		/// Объект <see cref="IDisposable" />, выполняющий действие <paramref name="dispose" /> при освобождении
		/// ресурсов.
		/// </returns>
		public IDisposable Action(Action dispose)
		{
			return new ActionDisposable(dispose);
		}

		#endregion
	}
}