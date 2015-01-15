#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Owin;

#endregion

namespace Bricks.OWIN.Middeware
{
	public class BasicAuthenticationMiddleware
	{
		private readonly Func<IDictionary<string, object>, Task> _next;
		private readonly BasicAuthenticationOptions _options;

		public BasicAuthenticationMiddleware(Func<IDictionary<string, object>, Task> next, BasicAuthenticationOptions options)
		{
			_next = next;
			_options = options;
		}

		public Task Invoke(IDictionary<string, object> environment)
		{
			var context = new OwinContext(environment);
			var isAuthenticated = false;

			var authorization = context.Request.Headers["Authorization"];
			if (!string.IsNullOrEmpty(authorization))
			{
				var authorizationParts = authorization.Split(' ');
				if (authorizationParts.Length == 2 && authorizationParts[0] == "Basic")
				{
					var usernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationParts[1]));
					var usernamePasswordParts = usernamePassword.Split(':');
					if (usernamePasswordParts.Length == 2
						&& usernamePasswordParts[0] == _options.Username
						&& usernamePasswordParts[1] == _options.Password)
					{
						isAuthenticated = true;
					}
				}
			}

			if (!isAuthenticated)
			{
				context.Response.Headers.Add("WWW-Authenticate", new[] { string.Format(CultureInfo.InvariantCulture, "Basic realm=\"{0}\"", _options.Realm) });
				context.Response.StatusCode = 401;
				return Task.FromResult((object)null);
			}

			return _next(environment);
		}
	}
}