#region

using Bricks.Auth.Implementation;

using NUnit.Framework;

#endregion

namespace Bricks.Auth.Tests
{
	public sealed class PasswordHasherTests
	{
		[TestCase("MKn0nS7CFafzkB0koq7V")]
		[TestCase("4iHc32QVrKLOx3lOC9aP")]
		[TestCase("usLStEKU0DnK8Njbs0WB")]
		public void HashPassword_SamePasswords_SameHashes(string password)
		{
			// Arrange
			var passwordHasher = new PasswordHasher();

			// Act
			var result1 = passwordHasher.HashPassword(password);
			var result2 = passwordHasher.HashPassword(password);

			// Assert
			Assert.AreEqual(result1, result2);
		}
	}
}