#region

using System.Collections.Generic;

#endregion

namespace Bricks.Core.Modularity
{
	/// <summary>
	/// Настройки модульности приложения.
	/// </summary>
	public interface IModularitySettings
	{
		/// <summary>
		/// Коллекция модулей приложения.
		/// </summary>
		IReadOnlyCollection<IModuleSettings> Modules { get; }
	}
}