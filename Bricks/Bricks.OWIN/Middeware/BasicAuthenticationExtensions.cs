#region

using Owin;

#endregion

namespace Bricks.OWIN.Middeware
{
	public static class BasicAuthenticationExtensions
	{
		public static void UseBasicAuthentication(this IAppBuilder appBuilder, BasicAuthenticationOptions options)
		{
			appBuilder.Use<BasicAuthenticationMiddleware>(options);
		}
	}
}