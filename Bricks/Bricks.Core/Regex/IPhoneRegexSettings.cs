namespace Bricks.Core.Regex
{
	/// <summary>
	/// Настройки regex для номера телефона.
	/// </summary>
	public interface IPhoneRegexSettings
	{
		/// <summary>
		/// Паттерн номера.
		/// </summary>
		string NumberPattern { get; }

		/// <summary>
		/// Формат номера.
		/// </summary>
		string NumberFormat { get; }

		/// <summary>
		/// Формат только номера (без кодов).
		/// </summary>
		string NumberOnlyFormat { get; }

		/// <summary>
		/// Паттерн игнорируемых символов.
		/// </summary>
		string NoisePattern { get; }
	}
}