#region

using System.Globalization;

using Microsoft.Owin;

#endregion

namespace Bricks.OWIN.Extensions
{
	public static class OwinRequestExtensions
	{
		private const string DefaultLocaleHeaderName = "Accept-Locale";

		public static CultureInfo GetCulture(this IOwinRequest owinRequest, string localeHeaderName = null)
		{
			if (string.IsNullOrEmpty(localeHeaderName))
			{
				localeHeaderName = DefaultLocaleHeaderName;
			}

			string localeName = owinRequest.Headers.Get(localeHeaderName);
			CultureInfo cultureInfo = null;
			if (!string.IsNullOrEmpty(localeName))
			{
				cultureInfo = CultureInfo.GetCultureInfo(localeName);
			}

			return cultureInfo;
		}
	}
}