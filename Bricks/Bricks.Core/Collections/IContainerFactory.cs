namespace Bricks.Core.Collections
{
	public interface IContainerFactory
	{
		IContainer<TKey, TValue> Create<TKey, TValue>();
	}
}