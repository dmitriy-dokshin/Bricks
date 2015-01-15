#region

using System;

#endregion

namespace Bricks.Core.Collections
{
	public interface IContainer<in TKey, TValue>
	{
		IDisposable GetOrAdd(TKey key, Func<TValue> createFunc, out TValue result);
	}
}