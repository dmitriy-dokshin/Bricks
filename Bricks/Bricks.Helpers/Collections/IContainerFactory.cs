namespace Bricks.Helpers.Collections
{
	public interface IContainerFactory
	{
		IContainer<TKey, TValue> Create<TKey, TValue>();
	}
}