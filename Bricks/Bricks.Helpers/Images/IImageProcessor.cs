using System.Drawing;

namespace Bricks.Helpers.Images
{
	public interface IImageProcessor
	{
		byte[] Resize(byte[] data, int? width = null, int? height = null, bool preserveAspectRatio = true);

		Image Resize(Image image, int? width = null, int? height = null, bool preserveAspectRatio = true);
	}
}