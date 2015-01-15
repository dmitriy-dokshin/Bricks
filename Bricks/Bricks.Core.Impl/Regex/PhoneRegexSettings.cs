#region

using System.Configuration;

using Bricks.Core.Regex;

#endregion

namespace Bricks.Core.Impl.Regex
{
	internal class PhoneRegexSettings : ConfigurationElement, IPhoneRegexSettings
	{
		private const string NumberPatternName = "numberPattern";
		private const string NumberFormatName = "numberFormat";
		private const string NumberOnlyFormatName = "numberOnlyFormat";
		private const string NoisePatternName = "noisePattern";

		[ConfigurationProperty(NumberPatternName, IsRequired = true)]
		public string NumberPattern
		{
			get
			{
				return (string)this[NumberPatternName];
			}
		}

		[ConfigurationProperty(NumberOnlyFormatName, IsRequired = true)]
		public string NumberOnlyFormat
		{
			get
			{
				return (string)this[NumberOnlyFormatName];
			}
		}

		[ConfigurationProperty(NumberFormatName, IsRequired = true)]
		public string NumberFormat
		{
			get
			{
				return (string)this[NumberFormatName];
			}
		}

		[ConfigurationProperty(NoisePatternName, IsRequired = true)]
		public string NoisePattern
		{
			get
			{
				return (string)this[NoisePatternName];
			}
		}

		#region Implementation of IPhoneRegexSettings

		/// <summary>
		/// Паттерн номера.
		/// </summary>
		string IPhoneRegexSettings.NumberPattern
		{
			get
			{
				return NumberPattern;
			}
		}

		/// <summary>
		/// Формат номера.
		/// </summary>
		string IPhoneRegexSettings.NumberFormat
		{
			get
			{
				return NumberFormat;
			}
		}

		/// <summary>
		/// Формат только номера (без кодов).
		/// </summary>
		string IPhoneRegexSettings.NumberOnlyFormat
		{
			get
			{
				return NumberOnlyFormat;
			}
		}

		/// <summary>
		/// Паттерн игнорируемых символов.
		/// </summary>
		string IPhoneRegexSettings.NoisePattern
		{
			get
			{
				return NoisePattern;
			}
		}

		#endregion
	}
}