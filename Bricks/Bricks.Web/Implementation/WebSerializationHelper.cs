#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.ServiceLocation;

using Newtonsoft.Json;

#endregion

namespace Bricks.Web.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IWebSerializationHelper" />.
	/// </summary>
	internal class WebSerializationHelper : IWebSerializationHelper
	{
		private readonly IServiceLocator _serviceLocator;
		private readonly IDictionary<Type, Type> _valueConverterTypesByTSource;

		public WebSerializationHelper(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
			_valueConverterTypesByTSource = new Dictionary<Type, Type>();
		}

		#region Implementation of IWebSerializationHelper

		/// <summary>
		/// Создаёт коллекцию ключей-значений для объекта <paramref name="source" />.
		/// </summary>
		/// <param name="source">Целевой объект.</param>
		/// <returns>Коллекция ключей-значений для объекта <paramref name="source" />.</returns>
		public NameValueCollection ToNameValueCollection(object source)
		{
			var nameValueCollection = new NameValueCollection();
			Type type = source.GetType();
			PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				if (propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() != null)
				{
					continue;
				}

				var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
				string name = jsonPropertyAttribute != null ? jsonPropertyAttribute.PropertyName : propertyInfo.Name;

				object value = propertyInfo.GetValue(source);
				if (value != null)
				{
					var valueConverterAttribute = propertyInfo.GetCustomAttribute<ValueConverterAttribute>();
					Type valueConverterType;
					if (valueConverterAttribute != null)
					{
						valueConverterType = valueConverterAttribute.ValueConverterType;
					}
					else
					{
						Type valueType = value.GetType();
						_valueConverterTypesByTSource.TryGetValue(valueType, out valueConverterType);
						if (valueConverterType == null)
						{
							IReadOnlyCollection<Type> valueConverterTypes =
							_valueConverterTypesByTSource.Where(x => x.Key.IsAssignableFrom(valueType)).Select(x => x.Value).ToArray();
							if (valueConverterTypes.Count > 1)
							{
								string message = string.Format(CultureInfo.InvariantCulture,
								ResourcesCore.WebSerializationHelper_ToNameValueCollection_MultipleValueConverterTypes_InvalidOperationExceptionMessage,
								valueType.FullName);
								throw new InvalidOperationException(message);
							}

							valueConverterType = valueConverterTypes.FirstOrDefault();
						}
					}

					if (valueConverterType != null)
					{
						var valueConverter = (IValueConverter)_serviceLocator.GetInstance(valueConverterType);
						value = valueConverter.Convert(value);
					}
				}

				if (value == null)
				{
					continue;
				}

				nameValueCollection.Add(name, value.ToString());
			}

			return nameValueCollection;
		}

		/// <summary>
		/// Регистрирует конвертер значений типа <typeparamref name="TSource" />.
		/// </summary>
		/// <typeparam name="TSource">Тип исходных значений.</typeparam>
		/// <typeparam name="TConverter">Тип конвертера.</typeparam>
		public void RegisterValueConverter<TSource, TConverter>() where TConverter : IValueConverter
		{
			_valueConverterTypesByTSource.Add(typeof(TSource), typeof(TConverter));
		}

		#endregion
	}
}