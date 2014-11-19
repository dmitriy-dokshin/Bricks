#region

using System.Configuration;

#endregion

namespace Bricks.Helpers.Regex.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IEmailRegexSettings" />.
	/// </summary>
	internal sealed class EmailRegexSettings : ConfigurationElement, IEmailRegexSettings
	{
		private const string PatternName = "pattern";

		#region Implementation of IEmailRegexSettings

		/// <summary>
		/// Паттерн email'а.
		/// </summary>
		[ConfigurationProperty(PatternName, IsRequired = true)]
		public string Pattern
		{
			get { return (string) this[PatternName]; }
		}

		#endregion
	}
}