#region

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Bricks.Sync;

#endregion

namespace Bricks.Helpers.Collections.Implementation
{
	internal sealed class CollectionHelper : ICollectionHelper
	{
		private readonly IInterlockedHelper _interlockedHelper;
		private IImmutableDictionary<Type, object> _emptyCollectionsByType;
		private IImmutableDictionary<Type, object> _emptyReadOnlyCollectionsByType;

		public CollectionHelper(IInterlockedHelper interlockedHelper)
		{
			_interlockedHelper = interlockedHelper;
			_emptyCollectionsByType = ImmutableDictionary.Create<Type, object>();
			_emptyReadOnlyCollectionsByType = ImmutableDictionary.Create<Type, object>();
		}

		#region Implementation of ICollectionHelper

		public ICollection<T> GetEmptyCollection<T>()
		{
			return _interlockedHelper.CompareExchange(ref _emptyCollectionsByType, x =>
				{
					Type type = typeof(T);
					IImmutableDictionary<Type, object> newValue = null;
					object collection;
					if (!_emptyReadOnlyCollectionsByType.TryGetValue(type, out collection))
					{
						collection = new T[0];
						newValue = x.Add(type, collection);
					}

					return _interlockedHelper.CreateChangeResult(newValue ?? x, (ICollection<T>)collection);
				});
		}

		public IReadOnlyCollection<T> GetEmptyReadOnlyCollection<T>()
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

					return _interlockedHelper.CreateChangeResult(newValue ?? x, (IReadOnlyCollection<T>)collection);
				});
		}

		public IEnumerable<T> Single<T>(T item)
		{
			yield return item;
		}

		#endregion
	}
}