#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Bricks.OWIN.Extensions;

using Microsoft.Owin;

#endregion

namespace Bricks.OWIN.Middeware
{
	public class GlobalizationMiddleware
	{
		private readonly Func<IDictionary<string, object>, Task> _next;
		private readonly GlobalizationOptions _options;

		public GlobalizationMiddleware(Func<IDictionary<string, object>, Task> next, GlobalizationOptions options = null)
		{
			_next = next;
			_options = options ?? new GlobalizationOptions();
		}

		public async Task Invoke(IDictionary<string, object> environment)
		{
			var context = new OwinContext(environment);
			CultureInfo cultureInfo = context.Request.GetCulture(_options.LocaleHeaderName);
			if (cultureInfo != null)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo;
				Thread.CurrentThread.CurrentUICulture = cultureInfo;
			}

			await _next(environment);
		}
	}
}