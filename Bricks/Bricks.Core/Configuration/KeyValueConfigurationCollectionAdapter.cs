#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

#endregion

namespace Bricks.Core.Configuration
{
	/// <summary>
	/// Адаптер <see cref="KeyValueConfigurationCollection" /> для <see cref="IReadOnlyDictionary{TKey,TValue}" />.
	/// </summary>
	/// <typeparam name="TValue">Тип значения словаря.</typeparam>
	public sealed class KeyValueConfigurationCollectionAdapter<TValue> : IReadOnlyDictionary<string, TValue>
	{
		private readonly Func<string, TValue> _createValue;
		private readonly Lazy<IEnumerable<KeyValuePair<string, TValue>>> _enumerableLazy;
		private readonly KeyValueConfigurationCollection _source;
		private readonly Lazy<IEnumerable<TValue>> _valueEnumerableLazy;

		public KeyValueConfigurationCollectionAdapter(KeyValueConfigurationCollection source, Func<string, TValue> createValue)
		{
			_source = source;
			_createValue = createValue;
			_enumerableLazy = new Lazy<IEnumerable<KeyValuePair<string, TValue>>>(() => source.AllKeys.Select(x => new KeyValuePair<string, TValue>(x, createValue(_source[x].Value))), false);
			_valueEnumerableLazy = new Lazy<IEnumerable<TValue>>(() => _source.AllKeys.Select(x => createValue(_source[x].Value)), false);
		}

		#region Implementation of IReadOnlyCollection<out KeyValuePair<string,TValue>>

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
		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			return _enumerableLazy.Value.GetEnumerator();
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

		#region Implementation of IReadOnlyDictionary<string,TValue>

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
		public bool TryGetValue(string key, out TValue value)
		{
			KeyValueConfigurationElement keyValueConfigurationElement = _source[key];
			if (keyValueConfigurationElement != null)
			{
				value = _createValue(keyValueConfigurationElement.Value);
				return true;
			}

			value = default (TValue);
			return false;
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
		public TValue this[string key]
		{
			get
			{
				TValue value;
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
		public IEnumerable<TValue> Values
		{
			get
			{
				return _valueEnumerableLazy.Value;
			}
		}

		#endregion
	}
}