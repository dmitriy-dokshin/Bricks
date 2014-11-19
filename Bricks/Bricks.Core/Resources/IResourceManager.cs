#region

using System.Globalization;

#endregion

namespace Bricks.Core.Resources
{
	/// <summary>
	/// Представляет менеджера ресурсов.
	/// </summary>
	public interface IResourceManager
	{
		/// <summary>
		/// Получает локализованную строку, соответствующую культуре <paramref name="culture" />,
		/// по имени <paramref name="name" />.
		/// </summary>
		/// <param name="name">Имя строки в ресурсе.</param>
		/// <param name="culture">Культура.</param>
		/// <returns>Локализованная строка.</returns>
		string GetString(string name, CultureInfo culture = null);
	}
}