#region

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

using Bricks.Core.Collections;
using Bricks.Core.Sync;

#endregion

namespace Bricks.Core.Impl.Collections
{
	internal sealed class CollectionHelper : ICollectionHelper
	{
		private readonly IInterlockedHelper _interlockedHelper;
		private IImmutableDictionary<Type, object> _emptyReadOnlyCollectionsByType;

		public CollectionHelper(IInterlockedHelper interlockedHelper)
		{
			_interlockedHelper = interlockedHelper;
			_emptyReadOnlyCollectionsByType = ImmutableDictionary.Create<Type, object>();
		}

		#region Implementation of ICollectionHelper

		public ICollection<T> GetEmptyCollection<T>()
		{
			return new Collection<T>();
		}

		public IList<T> GetEmptyList<T>()
		{
			return new List<T>();
		}

		public IReadOnlyCollection<T> GetEmptyReadOnlyCollection<T>()
		{
			return GetEmptyReadOnlyList<T>();
		}

		public IReadOnlyList<T> GetEmptyReadOnlyList<T>()
		{
			return _interlockedHelper.CompareExchange(ref _emptyReadOnlyCollectionsByType, x =>
				{
					Type type = typeof(T);
					IImmutableDictionary<Type, object> newValue = null;
					object collection;
					if (!_emptyReadOnlyCollectionsByType.TryGetValue(type, out collection))
					{
						collection = new T[0];
						newValue = x.Add(type, collection);
					}

					return _interlockedHelper.CreateChangeResult(newValue ?? x, (IReadOnlyList<T>)collection);
				});
		}

		public IEnumerable<T> Single<T>(T item)
		{
			yield return item;
		}

		#endregion
	}
}