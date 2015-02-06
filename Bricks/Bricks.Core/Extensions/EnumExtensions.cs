namespace Bricks.Core.Extensions
{
	public static class EnumExtensions
	{
		public static string GetName(this System.Enum value)
		{
			string name = System.Enum.GetName(value.GetType(), value);
			return name;
		}

		public static string GetLowerCamelCaseName(this System.Enum value)
		{
			string name = value.GetName();
			if (name.Length > 0 && !char.IsLower(name[0]))
			{
				name = char.ToLowerInvariant(name[0]) + (name.Length > 1 ? name.Substring(1) : string.Empty);
			}

			return name;
		}
	}
}