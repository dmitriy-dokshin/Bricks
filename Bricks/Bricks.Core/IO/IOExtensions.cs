#region

using System.IO;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.IO
{
	public static class IOExtensions
	{
		public static async Task<byte[]> GetBytes(this Stream stream)
		{
			var memoryStream = stream as MemoryStream;
			byte[] bytes;
			if (memoryStream != null)
			{
				bytes = memoryStream.ToArray();
			}
			else
			{
				using (memoryStream = new MemoryStream())
				{
					await stream.CopyToAsync(memoryStream);
					bytes = memoryStream.ToArray();
				}
			}

			return bytes;
		}
	}
}