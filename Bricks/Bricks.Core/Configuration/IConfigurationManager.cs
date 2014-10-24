#region

using System.Collections.Generic;

#endregion

namespace Bricks.Core.Configuration
{
	/// <summary>
	/// Интерфейс менеджера конфигурации.
	/// </summary>
	public interface IConfigurationManager
	{
		/// <summary>
		/// Настройки приложения.
		/// </summary>
		IReadOnlyDictionary<string, string> AppSettings { get; }

		/// <summary>
		/// Возвращает настройки типа <typeparamref name="TSettings" /> по ключу <see cref="key" />.
		/// </summary>
		/// <typeparam name="TSettings">Тип настроек.</typeparam>
		/// <param name="key">Ключ настроек.</param>
		/// <returns>Настройки типа <typeparamref name="TSettings" />.</returns>
		TSettings GetSettings<TSettings>(string key = null);

		/// <summary>
		/// Добавляет настройки типа <typeparamref name="TSettings" /> с необязательным ключом <see cref="key" />.
		/// </summary>
		/// <typeparam name="TSettings">Тип настроек.</typeparam>
		/// <param name="settings">Объект настроек.</param>
		/// <param name="key">Ключ настроек.</param>
		void AddSettings<TSettings>(TSettings settings, string key = null);
	}
}