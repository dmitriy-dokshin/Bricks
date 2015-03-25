#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

using Bricks.Core.Configuration;
using Bricks.Core.Conversion;
using Bricks.Core.IoC;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Impl.Configuration
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IConfigurationManager" />, использующая <see cref="ConfigurationManager" />.
	/// </summary>
	internal sealed class ConfigurationManagerImpl : IConfigurationManager
	{
		private readonly IConverter _converter;
		private readonly IServiceLocator _serviceLocator;
		private IImmutableDictionary<Type, IImmutableDictionary<string, object>> _settings;

		public ConfigurationManagerImpl(IServiceLocator serviceLocator, IConverter converter)
		{
			_serviceLocator = serviceLocator;
			_converter = converter;
			_settings = ImmutableDictionary<Type, IImmutableDictionary<string, object>>.Empty;
		}

		#region Nested type: NameValueCollectionAdapter

		/// <summary>
		/// Адаптер <see cref="NameValueCollection" /> для <see cref="IReadOnlyDictionary{TKey,TValue}" />.
		/// </summary>
		private class NameValueCollectionAdapter : IReadOnlyDictionary<string, string>
		{
			private readonly Lazy<IEnumerable<KeyValuePair<string, string>>> _keyValueEnumerableLazy;
			private readonly NameValueCollection _source;
			private readonly Lazy<IEnumerable<string>> _valueEnumerableLazy;

			public NameValueCollectionAdapter(NameValueCollection source)
			{
				_source = source;
				_keyValueEnumerableLazy = new Lazy<IEnumerable<KeyValuePair<string, string>>>(() => _source.AllKeys.Select(x => new KeyValuePair<string, string>(x, _source[x])), false);
				_valueEnumerableLazy = new Lazy<IEnumerable<string>>(() => _source.AllKeys.Select(x => _source[x]), false);
			}

			#region Implementation of IReadOnlyCollection<out KeyValuePair<string,string>>

			/// <summary>
			/// Gets the number of elements in the collection.
			/// </summary>
			/// <returns>
			/// The number of elements in the collection.
			/// </returns>
			public int Count
			{
				get
				{
					return _source.Count;
				}
			}

			#endregion

			#region Implementation of IEnumerable

			/// <summary>
			/// Returns an enumerator that iterates through the collection.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
			/// </returns>
			public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
			{
				return _keyValueEnumerableLazy.Value.GetEnumerator();
			}

			/// <summary>
			/// Returns an enumerator that iterates through a collection.
			/// </summary>
			/// <returns>
			/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
			/// </returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			#endregion

			#region Implementation of IReadOnlyDictionary<string,string>

			/// <summary>
			/// Determines whether the read-only dictionary contains an element that has the specified key.
			/// </summary>
			/// <returns>
			/// true if the read-only dictionary contains an element that has the specified key; otherwise, false.
			/// </returns>
			/// <param name="key">The key to locate.</param>
			/// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
			public bool ContainsKey(string key)
			{
				return _source.AllKeys.Contains(key);
			}

			/// <summary>
			/// Gets the value that is associated with the specified key.
			/// </summary>
			/// <returns>
			/// true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" /> interface
			/// contains an element that has the specified key; otherwise, false.
			/// </returns>
			/// <param name="key">The key to locate.</param>
			/// <param name="value">
			/// When this method returns, the value associated with the specified key, if the key is found;
			/// otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed
			/// uninitialized.
			/// </param>
			/// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
			public bool TryGetValue(string key, out string value)
			{
				value = _source[key];
				return value != null;
			}

			/// <summary>
			/// Gets the element that has the specified key in the read-only dictionary.
			/// </summary>
			/// <returns>
			/// The element that has the specified key in the read-only dictionary.
			/// </returns>
			/// <param name="key">The key to locate.</param>
			/// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
			/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
			/// The property is retrieved and
			/// <paramref name="key" /> is not found.
			/// </exception>
			public string this[string key]
			{
				get
				{
					string value;
					if (!TryGetValue(key, out value))
					{
						throw new KeyNotFoundException();
					}

					return value;
				}
			}

			/// <summary>
			/// Gets an enumerable collection that contains the keys in the read-only dictionary.
			/// </summary>
			/// <returns>
			/// An enumerable collection that contains the keys in the read-only dictionary.
			/// </returns>
			public IEnumerable<string> Keys
			{
				get
				{
					return _source.AllKeys;
				}
			}

			/// <summary>
			/// Gets an enumerable collection that contains the values in the read-only dictionary.
			/// </summary>
			/// <returns>
			/// An enumerable collection that contains the values in the read-only dictionary.
			/// </returns>
			public IEnumerable<string> Values
			{
				get
				{
					return _valueEnumerableLazy.Value;
				}
			}

			#endregion
		}

		#endregion

		#region Implementation of IConfigurationManager

		/// <summary>
		/// Настройки приложения.
		/// </summary>
		public IReadOnlyDictionary<string, string> AppSettings
		{
			get
			{
				return new NameValueCollectionAdapter(ConfigurationManager.AppSettings);
			}
		}

		/// <summary>
		/// Возвращает настройки типа <typeparamref name="TSettings" /> по ключу <see cref="key" />.
		/// </summary>
		/// <typeparam name="TSettings">Тип настроек.</typeparam>
		/// <param name="key">Ключ настроек.</param>
		/// <returns>Настройки типа <typeparamref name="TSettings" />.</returns>
		public TSettings GetSettings<TSettings>(string key = null)
		{
			if (key == null)
			{
				key = string.Empty;
			}

			IImmutableDictionary<string, object> settingsByKey;
			object settings = null;
			if ((!_settings.TryGetValue(typeof(TSettings), out settingsByKey) || !settingsByKey.TryGetValue(key, out settings)) && !string.IsNullOrEmpty(key))
			{
				settings = ConfigurationManager.GetSection(key);
			}

			TSettings result;
			if (settings != null)
			{
				if (!_converter.TryConvert(settings, out result))
				{
					result = (TSettings)settings;
				}
			}
			else
			{
				throw new InvalidOperationException();
			}

			_serviceLocator.BuildUp(result);

			return result;
		}

		/// <summary>
		/// Добавляет настройки типа <typeparamref name="TSettings" /> с необязательным ключом <see cref="key" />.
		/// </summary>
		/// <typeparam name="TSettings">Тип настроек.</typeparam>
		/// <param name="settings">Объект настроек.</param>
		/// <param name="key">Ключ настроек.</param>
		public void AddSettings<TSettings>(TSettings settings, string key = null)
		{
			if (key == null)
			{
				key = string.Empty;
			}

			Type type = typeof(TSettings);
			IImmutableDictionary<string, object> settingsByKey;
			if (!_settings.TryGetValue(type, out settingsByKey))
			{
				settingsByKey = ImmutableDictionary<string, object>.Empty;
			}

			settingsByKey = settingsByKey.SetItem(key, settings);
			_settings = _settings.SetItem(type, settingsByKey);
		}

		#endregion
	}
}