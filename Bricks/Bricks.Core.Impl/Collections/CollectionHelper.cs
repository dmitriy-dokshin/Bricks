#region

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Bricks.Core.Collections;
using Bricks.Core.Sync;

#endregion

namespace Bricks.Core.Impl.Collections
{
	internal sealed class CollectionHelper : ICollectionHelper
	{
		private IImmutableDictionary<Type, object> _emptyCollectionsByType;
		private IImmutableDictionary<Type, object> _emptyReadOnlyCollectionsByType;
		private readonly IInterlockedHelper _interlockedHelper;

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
					var type = typeof(T);
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
					var type = typeof(T);
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