namespace Bricks.Core.Extensions
{
	public static class ObjectExtensions
	{
		public static string ToStringOrNull(this object source)
		{
			return source != null ? source.ToString() : null;
		}
	}
}