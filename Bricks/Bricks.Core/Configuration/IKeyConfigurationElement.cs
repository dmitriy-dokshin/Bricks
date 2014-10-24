namespace Bricks.Core.Configuration
{
	/// <summary>
	/// Элемент конфигурации, у которого есть ключ.
	/// </summary>
	/// <typeparam name="TKey">Тип ключа.</typeparam>
	public interface IKeyConfigurationElement<out TKey>
	{
		/// <summary>
		/// Ключ элемента.
		/// </summary>
		TKey Key { get; }
	}
}