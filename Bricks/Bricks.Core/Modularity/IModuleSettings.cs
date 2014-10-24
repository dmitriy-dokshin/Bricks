using System;

namespace Bricks.Core.Modularity
{
	/// <summary>
	/// Настройки модуля приложения.
	/// </summary>
	public interface IModuleSettings
	{
		/// <summary>
		/// Имя модуля.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Тип модуля.
		/// </summary>
		Type Type { get; }

		/// <summary>
		/// Порядок инициализации.
		/// </summary>
		int Order { get; }
	}
}