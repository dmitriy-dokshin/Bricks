#region

using System;
using System.Globalization;

#endregion

namespace Bricks.Auth.Implementation
{
	internal sealed class CodeGenerator : ICodeGenerator
	{
		private readonly Random _random;

		public CodeGenerator()
		{
			_random = new Random();
		}

		#region Implementation of ICodeGenerator

		public string CreateNumericCode(int length)
		{
			var code = new char[length];
			for (int i = 0; i < length; i++)
			{
				code[i] = _random.Next(10).ToString(CultureInfo.InvariantCulture)[0];
			}

			return new string(code);
		}

		#endregion
	}
}