namespace Bricks.Core.Auth.ExternalLogins
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