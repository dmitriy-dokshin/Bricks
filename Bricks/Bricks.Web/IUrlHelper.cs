#region

using System;

#endregion

namespace Bricks.Web
{
	/// <summary>
	/// Содержит вспомогательные методы для работы с <see cref="Uri" />.
	/// </summary>
	public interface IUrlHelper
	{
		/// <summary>
		/// Получает URL <paramref name="address" /> с параметрами <paramref name="parameters" />.
		/// </summary>
		/// <typeparam name="TParameters">Тип параметров.</typeparam>
		/// <param name="address">Адрес web-сервиса.</param>
		/// <param name="parameters">Параметры.</param>
		/// <returns>URL с параметрами.</returns>
		Uri GetUrl<TParameters>(Uri address, TParameters parameters);
	}
}