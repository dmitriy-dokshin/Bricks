#region

using System.IO;

using Bricks.Core.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Bricks.Core.Impl.Serialization
{
	/// <summary>
	/// Реализация по умолчанию <see cref="ISerializationHelper" />.
	/// </summary>
	internal sealed class SerializationHelper : ISerializationHelper
	{
		private readonly JsonSerializer _jsonSerializer;

		public SerializationHelper(JsonSerializer jsonSerializer)
		{
			_jsonSerializer = jsonSerializer;
		}

		#region Implementation of ISerializationHelper

		/// <summary>
		/// Создаёт объект <see cref="JObject" /> на основе объекта <paramref name="source" />.
		/// </summary>
		/// <typeparam name="T">Тип исходного объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект <see cref="JObject" />.</returns>
		public JObject CreateJObject<T>(T source)
		{
			return JObject.FromObject(source, _jsonSerializer);
		}

		/// <summary>
		/// Создаёт объект <see cref="JToken" /> на основе объекта <paramref name="source" />.
		/// </summary>
		/// <typeparam name="T">Тип исходного объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект <see cref="JToken" />.</returns>
		public JToken CreateJToken<T>(T source)
		{
			return JToken.FromObject(source, _jsonSerializer);
		}

		/// <summary>
		/// Выполняет десериализацию объекта из потока с данными в формате JSON.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="stream">Поток с данными в формате JSON.</param>
		/// <returns>Новый объект <see cref="T" />.</returns>
		public T DeserializeJson<T>(Stream stream)
		{
			using (var streamReader = new StreamReader(stream))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
				{
					var result = _jsonSerializer.Deserialize<T>(jsonReader);
					return result;
				}
			}
		}

		/// <summary>
		/// Выполняет десериализацию объекта из текста в формате JSON.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="json">Текст с данными в формате JSON.</param>
		/// <returns>Новый объект <see cref="T" />.</returns>
		public T DeserializeJson<T>(string json)
		{
			using (var stringReader = new StringReader(json))
			{
				using (var jsonTextReader = new JsonTextReader(stringReader))
				{
					var result = _jsonSerializer.Deserialize<T>(jsonTextReader);
					return result;
				}
			}
		}

		/// <summary>
		/// Выполняет десериализацию объекта из JSON-объекта.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="jObject">JSON-объект.</param>
		/// <returns>Новый объект <see cref="T" />.</returns>
		public T DeserializeJson<T>(JObject jObject)
		{
			return jObject.ToObject<T>(_jsonSerializer);
		}

		/// <summary>
		/// Пытается десериализовать объект из потока с данными в формате JSON.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="stream">Поток с данными в формате JSON.</param>
		/// <param name="value">Новый объект <see cref="T" />.</param>
		/// <returns>Признак успешной десериализации.</returns>
		public bool TryDeserializeJson<T>(Stream stream, out T value)
		{
			using (var streamReader = new StreamReader(stream))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
				{
					try
					{
						value = _jsonSerializer.Deserialize<T>(jsonReader);
						return true;
					}
					catch (JsonSerializationException)
					{
						value = default(T);
						return false;
					}
				}
			}
		}

		#endregion
	}
}