#region

using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Bricks.Core.Serialization;
using Bricks.Core.Sync;

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
		private IImmutableDictionary<Type, XmlSerializer> _xmlSerializersByType;
		private readonly IInterlockedHelper _interlockedHelper;
		private readonly JsonSerializer _jsonSerializer;

		public SerializationHelper(JsonSerializer jsonSerializer, IInterlockedHelper interlockedHelper)
		{
			_jsonSerializer = jsonSerializer;
			_interlockedHelper = interlockedHelper;
			_xmlSerializersByType = ImmutableDictionary<Type, XmlSerializer>.Empty;
		}

		#region Implementation of ISerializationHelper

		/// <summary>
		/// Создаёт объект <see cref="JObject" /> на основе объекта <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект <see cref="JObject" />.</returns>
		public JObject CreateJObject(object source)
		{
			return JObject.FromObject(source, _jsonSerializer);
		}

		/// <summary>
		/// Создаёт объект <see cref="JToken" /> на основе объекта <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект <see cref="JToken" />.</returns>
		public JToken CreateJToken(object source)
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

		public string SerializeToJson(object source)
		{
			var stringBuilder = new StringBuilder();
			using (TextWriter textWriter = new StringWriter(stringBuilder))
			{
				_jsonSerializer.Serialize(textWriter, source);
			}

			return stringBuilder.ToString();
		}

		public string SerializeToXml(object source)
		{
			if (source == null)
			{
				return null;
			}

			XmlSerializer xmlSerializer = GetXmlSerializer(source.GetType());
			StringBuilder stringBuilder = new StringBuilder();
			using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
			{
				xmlSerializer.Serialize(xmlWriter, source);
			}

			return stringBuilder.ToString();
		}

		public T DeserializeXml<T>(string xml)
		{
			if (xml == null)
			{
				return default(T);
			}

			return (T)DeserializeXml(xml, typeof(T));
		}

		public object DeserializeXml(string xml, Type type)
		{
			if (xml == null)
			{
				return null;
			}

			XmlSerializer xmlSerializer = GetXmlSerializer(type);
			using (var stringReader = new StringReader(xml))
			{
				return xmlSerializer.Deserialize(stringReader);
			}
		}

		private XmlSerializer GetXmlSerializer(Type type)
		{
			return _interlockedHelper.CompareExchange(ref _xmlSerializersByType, x =>
				{
					XmlSerializer xmlSerializer;
					IImmutableDictionary<Type, XmlSerializer> newValue;
					if (!x.TryGetValue(type, out xmlSerializer))
					{
						xmlSerializer = new XmlSerializer(type);
						newValue = x.Add(type, xmlSerializer);
					}
					else
					{
						newValue = x;
					}

					return _interlockedHelper.CreateChangeResult(newValue, xmlSerializer);
				});
		}

		#endregion
	}
}