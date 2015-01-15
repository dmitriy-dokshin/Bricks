namespace Bricks.Core.Auth
{
	public interface IPasswordHasher
	{
		string HashPassword(string password, byte[] salt = null);
	}
}