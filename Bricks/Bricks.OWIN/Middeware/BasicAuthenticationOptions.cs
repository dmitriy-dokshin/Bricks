namespace Bricks.OWIN.Middeware
{
	public class BasicAuthenticationOptions
	{
		public BasicAuthenticationOptions(string realm, string username, string password)
		{
			Realm = realm;
			Username = username;
			Password = password;
		}

		public string Realm { get; private set; }

		public string Username { get; private set; }

		public string Password { get; private set; }
	}
}