#region

using System.IO;

#endregion

namespace Bricks.Core.Helpers
{
	public static class ResourceHelper
	{
		public static string ReadString<T>(string resourceName)
		{
			using (Stream stream = typeof(T).Assembly.GetManifestResourceStream(resourceName))
			{
				if (stream != null)
				{
					using (var streamReader = new StreamReader(stream))
					{
						return streamReader.ReadToEnd();
					}
				}
			}

			return null;
		}
	}
}