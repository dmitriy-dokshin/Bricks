namespace Bricks.Core.Impl.Auth.ExternalLogins.VK
{
	/// <summary>
	/// Настройки VK.
	/// </summary>
	public interface IVkSettings
	{
		/// <summary>
		/// Идентификатор приложения.
		/// </summary>
		string ClientId { get; }

		/// <summary>
		/// Секретный ключ приложения.
		/// </summary>
		string ClientSecret { get; }
	}
}