#region

using System;
using System.Text;

#endregion

namespace Bricks.Core.Extensions
{
	public static class StringExtensions
	{
		public static string FromUnderscoreToCamelCase(this string source)
		{
			var resultBuilder = new StringBuilder();
			string[] words = source.Split(new[] { '_' }, StringSplitOptions.None);
			foreach (string word in words)
			{
				for (var i = 0; i < word.Length; i++)
				{
					char c = word[i];
					resultBuilder.Append(i == 0 ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
				}
			}

			string result = resultBuilder.ToString();
			return result;
		}
	}
}