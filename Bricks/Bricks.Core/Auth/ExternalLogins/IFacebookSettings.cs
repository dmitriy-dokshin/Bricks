namespace Bricks.Core.Auth.ExternalLogins
{
	/// <summary>
	/// Настройки Facebook.
	/// </summary>
	public interface IFacebookSettings
	{
		/// <summary>
		/// Идентификатор приложения.
		/// </summary>
		string AppId { get; }

		/// <summary>
		/// Секретный ключ приложения.
		/// </summary>
		string AppSecret { get; }
	}
}