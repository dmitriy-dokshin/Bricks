#region

using System;
using System.Security.Cryptography;
using System.Text;

using Bricks.Core.Auth;

#endregion

namespace Bricks.Core.Impl.Auth
{
	internal class PasswordHasher : IPasswordHasher
	{
		private const int Seed = 47;
		private const int SaltSize = 64;
		private readonly byte[] _defaultSalt;

		public PasswordHasher()
		{
			var random = new Random(Seed);
			_defaultSalt = new byte[SaltSize];
			random.NextBytes(_defaultSalt);
		}

		#region Implementation of IPasswordHasher

		public string HashPassword(string password, byte[] salt = null)
		{
			var plainText = Encoding.Unicode.GetBytes(password);
			if (salt == null)
			{
				salt = _defaultSalt;
			}

			HashAlgorithm algorithm = new SHA256Managed();

			var plainTextWithSaltBytes =
				new byte[plainText.Length + salt.Length];

			for (var i = 0; i < plainText.Length; i++)
			{
				plainTextWithSaltBytes[i] = plainText[i];
			}
			for (var i = 0; i < salt.Length; i++)
			{
				plainTextWithSaltBytes[plainText.Length + i] = salt[i];
			}

			var hash = algorithm.ComputeHash(plainTextWithSaltBytes);
			return Convert.ToBase64String(hash);
		}

		#endregion
	}
}