#region

using System.Drawing;

#endregion

namespace Bricks.Core.Images
{
	public interface IImageProcessor
	{
		byte[] Resize(byte[] data, int? width = null, int? height = null, bool preserveAspectRatio = true);

		Image Resize(Image image, int? width = null, int? height = null, bool preserveAspectRatio = true);
	}
}