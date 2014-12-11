#region

using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Bricks.Auth.Implementation
{
	internal class PasswordHasher : IPasswordHasher
	{
		private const int Seed = 47;
		private const int SaltSize = 64;
		private readonly byte[] _defaultSalt;

		public PasswordHasher()
		{
			Random random = new Random(Seed);
			_defaultSalt = new byte[SaltSize];
			random.NextBytes(_defaultSalt);
		}

		#region Implementation of IPasswordHasher

		public string HashPassword(string password, byte[] salt = null)
		{
			byte[] plainText = Encoding.Unicode.GetBytes(password);
			if (salt == null)
			{
				salt = _defaultSalt;
			}

			HashAlgorithm algorithm = new SHA256Managed();

			byte[] plainTextWithSaltBytes =
				new byte[plainText.Length + salt.Length];

			for (int i = 0; i < plainText.Length; i++)
			{
				plainTextWithSaltBytes[i] = plainText[i];
			}
			for (int i = 0; i < salt.Length; i++)
			{
				plainTextWithSaltBytes[plainText.Length + i] = salt[i];
			}

			var hash = algorithm.ComputeHash(plainTextWithSaltBytes);
			return Convert.ToBase64String(hash);
		}

		#endregion
	}
}