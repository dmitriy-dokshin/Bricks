namespace Bricks.Core.Auth.ExternalLogins
{
	public interface IExternalLoginData
	{
		string Key { get; }

		string FirstName { get; }

		string LastName { get; }

		string Email { get; }

		string About { get; }

		string ImageUrl { get; }
	}
}