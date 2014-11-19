namespace Bricks.Auth
{
	public interface IPasswordHasher
	{
		string HashPassword(string password);
	}
}