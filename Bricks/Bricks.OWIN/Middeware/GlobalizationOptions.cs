namespace Bricks.OWIN.Middeware
{
	public sealed class GlobalizationOptions
	{
		public GlobalizationOptions(string localeHeaderName = null)
		{
			LocaleHeaderName = localeHeaderName;
		}

		public string LocaleHeaderName { get; private set; }
	}
}