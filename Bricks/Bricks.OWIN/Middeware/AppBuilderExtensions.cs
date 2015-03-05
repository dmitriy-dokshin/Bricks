#region

using Owin;

#endregion

namespace Bricks.OWIN.Middeware
{
	public static class AppBuilderExtensions
	{
		public static void UseBasicAuthentication(this IAppBuilder appBuilder, BasicAuthenticationOptions options)
		{
			appBuilder.Use<BasicAuthenticationMiddleware>(options);
		}

		public static void UseGlobalization(this IAppBuilder appBuilder, GlobalizationOptions options = null)
		{
			appBuilder.Use<GlobalizationMiddleware>(options);
		}
	}
}