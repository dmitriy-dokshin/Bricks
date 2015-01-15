#region

using System.Configuration;

using Bricks.Core.Regex;

#endregion

namespace Bricks.Core.Impl.Regex
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
			get
			{
				return (string)this[PatternName];
			}
		}

		#endregion
	}
}