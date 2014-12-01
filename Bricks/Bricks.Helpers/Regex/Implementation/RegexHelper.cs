#region

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Bricks.Core.Configuration;
using Bricks.Core.Exceptions;
using Bricks.Core.Results;

#endregion

namespace Bricks.Helpers.Regex.Implementation
{
	/// <summary>
	/// Содержит вспомогательные методы для работы с регулярными выражениями.
	/// </summary>
	public class RegexHelper : IRegexHelper
	{
		private const string RegexSettingsKey = "regexSettings";
		private readonly IExceptionHelper _exceptionHelper;
		private readonly IRegexSettings _regexSettings;

		public RegexHelper(IConfigurationManager configurationManager, IExceptionHelper exceptionHelper)
		{
			_exceptionHelper = exceptionHelper;
			_regexSettings = configurationManager.GetSettings<IRegexSettings>(RegexSettingsKey);
		}

		/// <summary>
		/// Проверяет корректность email адреса <see cref="source" />.
		/// </summary>
		/// <param name="source">Строка с email адресом.</param>
		/// <returns>Признак корректного email'а.</returns>
		/// <remarks>
		/// Взято отсюда:
		/// http://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx
		/// </remarks>
		public bool IsValidEmail(string source)
		{
			string email;
			return TryParseEmail(source, out email);
		}

		/// <summary>
		/// Пытается разобрать email.
		/// </summary>
		/// <param name="source">Строка с email адресом.</param>
		/// <param name="email">Результат разбора email'а.</param>
		/// <returns>Признак успешного разбора email'а.</returns>
		public bool TryParseEmail(string source, out string email)
		{
			if (!String.IsNullOrEmpty(source))
			{
				var domainMapper = new DomainMapper();
				source = Replace(source, @"(@)(.+)$", domainMapper.Evaluate);
				if (domainMapper.IsValid)
				{
					Match match = Match(source, _regexSettings.Email.Pattern);
					if (match != null && match.Success)
					{
						email = source;
						return true;
					}
				}
			}

			email = null;
			return false;
		}

		/// <summary>
		/// Проверяет корректность номера телефона <paramref name="source" />.
		/// </summary>
		/// <param name="source">Сткрока с номером телефона.</param>
		/// <returns>Признак корректного номера телефона.</returns>
		public bool IsValidPhoneNumber(string source)
		{
			Match match = MatchPhoneNumber(source);
			return match != null && match.Success;
		}

		/// <summary>
		/// Пытается разобрать номер телефона из строки <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный текст с номером телефона.</param>
		/// <param name="phoneNumber">Результат разбора номера телефона.</param>
		/// <returns>Признак успешного разбора номера телефона.</returns>
		public bool TryParsePhoneNumber(string source, out string phoneNumber)
		{
			IPhoneRegexSettings phoneRegexSettings = _regexSettings.Phone;
			string numberFormat = phoneRegexSettings.NumberFormat;
			string numberOnlyFormat = phoneRegexSettings.NumberOnlyFormat;
			string countryCode;
			string cityCode;
			string number;
			bool success = TryParsePhoneNumber(source, out countryCode, out cityCode, out number);
			if (success)
			{
				var numberBuilder = new StringBuilder();
				int i = 0;
				foreach (char @char in numberOnlyFormat)
				{
					if (@char.Equals('n'))
					{
						numberBuilder.Append(number[i]);
						i++;
					}
					else
					{
						numberBuilder.Append(@char);
					}
				}

				phoneNumber = string.Format(numberFormat, countryCode, cityCode, numberBuilder);
			}
			else
			{
				phoneNumber = null;
			}

			return success;
		}

		/// <summary>
		/// Пытается разобрать номер телефона из строки <paramref name="source" />.
		/// </summary>
		/// <param name="source">Исходный текст с номером телефона.</param>
		/// <param name="country">Код страны (без "+").</param>
		/// <param name="area">Код города, региона или оператора.</param>
		/// <param name="number">Номер телефона.</param>
		/// <returns>Признак успешного разбора номера телефона.</returns>
		public bool TryParsePhoneNumber(string source, out string country, out string area, out string number)
		{
			const string CountrycodeGroupName = "country";
			const string CityCodeGroupName = "area";
			const string NumberGroupName = "number";

			country = null;
			area = null;
			number = null;

			Match match = MatchPhoneNumber(source);
			bool success = match.Success;
			if (success)
			{
				Group countryGroup = match.Groups[CountrycodeGroupName];
				country = countryGroup.Success ? countryGroup.Value : "+7";

				Group areaGroup = match.Groups[CityCodeGroupName];
				success = areaGroup.Success;
				if (success)
				{
					area = areaGroup.Value;
					Group numberGroup = match.Groups[NumberGroupName];
					success = numberGroup.Success;
					if (success)
					{
						number = numberGroup.Value;
					}
				}
			}

			return success;
		}

		#region Nested type: DomainMapper

		private class DomainMapper
		{
			public DomainMapper()
			{
				IsValid = true;
			}

			public bool IsValid { get; private set; }

			public string Evaluate(Match match)
			{
				var idn = new IdnMapping();
				string domainName = match.Groups[2].Value;
				try
				{
					domainName = idn.GetAscii(domainName);
				}
				catch (ArgumentException)
				{
					IsValid = false;
				}

				return match.Groups[1].Value + domainName;
			}
		}

		#endregion

		private Match MatchPhoneNumber(string source)
		{
			IPhoneRegexSettings phoneRegexSettings = _regexSettings.Phone;
			source = Replace(source, phoneRegexSettings.NoisePattern, string.Empty);
			Match match = source != null ? Match(source, phoneRegexSettings.NumberPattern) : null;
			return match;
		}

		private Match Match(string input, string pattern, RegexOptions options = RegexOptions.None)
		{
			IResult<Match> result = _exceptionHelper.Catch<Match, RegexMatchTimeoutException>(
				() => System.Text.RegularExpressions.Regex.Match(input, pattern, options, _regexSettings.MatchTimeout));
			return result.Success ? result.Data : null;
		}

		private string Replace(string input, string pattern, string replacement, RegexOptions options = RegexOptions.None)
		{
			IResult<string> result = _exceptionHelper.Catch<string, RegexMatchTimeoutException>(
				() => System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options, _regexSettings.MatchTimeout));
			return result.Success ? result.Data : null;
		}

		private string Replace(string input, string pattern, MatchEvaluator matchEvaluator, RegexOptions options = RegexOptions.None)
		{
			IResult<string> result = _exceptionHelper.Catch<string, RegexMatchTimeoutException>(
				() => System.Text.RegularExpressions.Regex.Replace(input, pattern, matchEvaluator, options, _regexSettings.MatchTimeout));
			return result.Success ? result.Data : null;
		}
	}
}