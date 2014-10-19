#region

using System;

#endregion

namespace Bricks.Core.Disposing
{
	/// <summary>
	/// Содержит вспомогательные методы для работы с классом <see cref="IDisposable" />.
	/// </summary>
	public interface IDisposableHelper
	{
		/// <summary>
		/// Создаёт объект <see cref="IDisposable" />, выполняющий действие <paramref name="dispose" /> при освобождении
		/// ресурсов.
		/// </summary>
		/// <param name="dispose">Действие, выполняемое при освобождении ресурсов.</param>
		/// <returns>
		/// Объект <see cref="IDisposable" />, выполняющий действие <paramref name="dispose" /> при освобождении
		/// ресурсов.
		/// </returns>
		IDisposable Action(Action dispose);
	}
}