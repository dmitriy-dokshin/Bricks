#region

using System;
using System.Security.Cryptography;

#endregion

namespace Bricks.Auth.Implementation
{
	internal class PasswordHasher : IPasswordHasher
	{
		private const int IterationsCount = 1000;
		private const int SaltSize = 16;
		private const int BytesRequired = 16;

		#region Implementation of IPasswordHasher

		public string HashPassword(string password)
		{
			var array = new byte[1 + SaltSize + BytesRequired];
			using (var pbkdf2 = new Rfc2898DeriveBytes(password, SaltSize, IterationsCount))
			{
				byte[] salt = pbkdf2.Salt;
				Buffer.BlockCopy(salt, 0, array, 1, SaltSize);
				byte[] bytes = pbkdf2.GetBytes(BytesRequired);
				Buffer.BlockCopy(bytes, 0, array, SaltSize + 1, BytesRequired);
			}
			return Convert.ToBase64String(array);
		}

		#endregion
	}
}