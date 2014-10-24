#region

using System.Collections;
using System.Collections.Generic;
using System.Configuration;

#endregion

namespace Bricks.Core.Configuration.Implementation
{
	/// <summary>
	/// Адаптер <see cref="ConfigurationElementCollection" /> для <see cref="IReadOnlyCollection{T}" />.
	/// </summary>
	/// <typeparam name="T">Тип элементов в коллекции.</typeparam>
	/// <typeparam name="TKey">Тип ключа элементов в коллекции.</typeparam>
	public class ConfigurationElementCollectionAdapter<T, TKey> : ConfigurationElementCollection, IReadOnlyCollection<T>
		where T : ConfigurationElement, IKeyConfigurationElement<TKey>, new()
	{
		#region Implementation of IEnumerable<out T>

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			IEnumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return (T)enumerator.Current;
			}
		}

		#endregion

		#region Overrides of ConfigurationElementCollection

		/// <summary>
		/// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Configuration.ConfigurationElement" />.
		/// </returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new T();
		}

		/// <summary>
		/// Gets the element key for a specified configuration element when overridden in a derived class.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Object" /> that acts as the key for the specified
		/// <see cref="T:System.Configuration.ConfigurationElement" />.
		/// </returns>
		/// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for. </param>
		protected override object GetElementKey(ConfigurationElement element)
		{
			var keyConfigurationElement = (IKeyConfigurationElement<TKey>)element;
			return keyConfigurationElement.Key;
		}

		#endregion
	}
}