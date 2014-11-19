#region

using System;
using System.Collections.Immutable;

using Bricks.Core.Disposing;
using Bricks.Sync;

#endregion

namespace Bricks.Helpers.Collections.Implementation
{
	internal sealed class Container<TKey, TValue> : IContainer<TKey, TValue>
	{
		private readonly IDisposableHelper _disposableHelper;
		private readonly IInterlockedHelper _interlockedHelper;
		private IImmutableDictionary<TKey, ValueCount<TValue>> _keyValuePairs;

		public Container(IInterlockedHelper interlockedHelper, IDisposableHelper disposableHelper)
		{
			_interlockedHelper = interlockedHelper;
			_disposableHelper = disposableHelper;
			_keyValuePairs = ImmutableDictionary.Create<TKey, ValueCount<TValue>>();
		}

		#region Implementation of IContainer<in TKey,TValue>

		public IDisposable GetOrAdd(TKey key, Func<TValue> createFunc, out TValue result)
		{
			result = _interlockedHelper.CompareExchange(ref _keyValuePairs, x =>
				{
					ValueCount<TValue> valueCount =
						x.TryGetValue(key, out valueCount)
							? valueCount.Increment()
							: new ValueCount<TValue>(createFunc());
					return _interlockedHelper.CreateChangeResult(x.SetItem(key, valueCount), valueCount.Value);
				});
			return _disposableHelper.Action(() => _interlockedHelper.CompareExchange(ref _keyValuePairs, x =>
				{
					ValueCount<TValue> valueCount = x[key];
					return valueCount.Count > 1 ? x.SetItem(key, valueCount.Decrement()) : x.Remove(key);
				}));
		}

		#endregion

		private sealed class ValueCount<T>
		{
			public ValueCount(T value)
				: this(value, 1)
			{
			}

			private ValueCount(T value, int count)
			{
				Value = value;
				Count = count;
			}

			public T Value { get; private set; }

			public int Count { get; private set; }

			public ValueCount<T> Increment()
			{
				return new ValueCount<T>(Value, Count + 1);
			}

			public ValueCount<T> Decrement()
			{
				return new ValueCount<T>(Value, Count - 1);
			}
		}
	}
}