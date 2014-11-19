#region

using System.Collections.Specialized;

#endregion

namespace Bricks.Web
{
	/// <summary>
	/// Содержит вспомогательные методы сериализации.
	/// </summary>
	public interface IWebSerializationHelper
	{
		/// <summary>
		/// Создаёт коллекцию ключей-значений для объекта <paramref name="source" />.
		/// </summary>
		/// <param name="source">Целевой объект.</param>
		/// <returns>Коллекция ключей-значений для объекта <paramref name="source" />.</returns>
		NameValueCollection ToNameValueCollection(object source);

		/// <summary>
		/// Регистрирует конвертер значений типа <typeparamref name="TSource" />.
		/// </summary>
		/// <typeparam name="TSource">Тип исходных значений.</typeparam>
		/// <typeparam name="TConverter">Тип конвертера.</typeparam>
		void RegisterValueConverter<TSource, TConverter>() where TConverter : IValueConverter;
	}
}