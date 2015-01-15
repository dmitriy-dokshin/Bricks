#region

using System;
using System.Globalization;

using Bricks.Core.Auth;

#endregion

namespace Bricks.Core.Impl.Auth
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
			for (var i = 0; i < length; i++)
			{
				code[i] = _random.Next(10).ToString(CultureInfo.InvariantCulture)[0];
			}

			return new string(code);
		}

		#endregion
	}
}