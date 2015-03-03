#region

using System;
using System.IO;

using Newtonsoft.Json.Linq;

#endregion

namespace Bricks.Core.Serialization
{
	/// <summary>
	/// Помощник сериализации.
	/// </summary>
	public interface ISerializationHelper
	{
		/// <summary>
		/// Создаёт объект <see cref="JObject" /> на основе объекта <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект <see cref="JObject" />.</returns>
		JObject CreateJObject(object source);

		/// <summary>
		/// Создаёт объект <see cref="JToken" /> на основе объекта <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект <see cref="JToken" />.</returns>
		JToken CreateJToken(object source);

		/// <summary>
		/// Выполняет десериализацию объекта из потока с данными в формате JSON.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="stream">Поток с данными в формате JSON.</param>
		/// <returns>Новый объект <see cref="T" />.</returns>
		T DeserializeJson<T>(Stream stream);

		/// <summary>
		/// Выполняет десериализацию объекта из текста в формате JSON.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="json">Текст с данными в формате JSON.</param>
		/// <returns>Новый объект <see cref="T" />.</returns>
		T DeserializeJson<T>(string json);

		/// <summary>
		/// Выполняет десериализацию объекта из JSON-объекта.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="jObject">JSON-объект.</param>
		/// <returns>Новый объект <see cref="T" />.</returns>
		T DeserializeJson<T>(JObject jObject);

		/// <summary>
		/// Пытается десериализовать объект из потока с данными в формате JSON.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="stream">Поток с данными в формате JSON.</param>
		/// <param name="value">Новый объект <see cref="T" />.</param>
		/// <returns>Признак успешной десериализации.</returns>
		bool TryDeserializeJson<T>(Stream stream, out T value);

		string SerializeToJson(object source);

		string SerializeToXml(object source);

		T DeserializeXml<T>(string xml);

		object DeserializeXml(string xml, Type type);
	}
}