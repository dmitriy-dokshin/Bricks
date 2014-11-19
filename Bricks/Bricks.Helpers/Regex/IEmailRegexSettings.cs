namespace Bricks.Helpers.Regex
{
	/// <summary>
	/// Настройки регулярных выражений для email'а.
	/// </summary>
	public interface IEmailRegexSettings
	{
		/// <summary>
		/// Паттерн email'а.
		/// </summary>
		string Pattern { get; }
	}
}