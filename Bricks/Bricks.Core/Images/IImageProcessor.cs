#region

using System.Drawing;
using System.Drawing.Imaging;

#endregion

namespace Bricks.Core.Images
{
	public interface IImageProcessor
	{
		byte[] Resize(byte[] data, int? width = null, int? height = null, bool preserveAspectRatio = true, ImageFormat format = null);

		Image Resize(Image image, int? width = null, int? height = null, bool preserveAspectRatio = true);
	}
}