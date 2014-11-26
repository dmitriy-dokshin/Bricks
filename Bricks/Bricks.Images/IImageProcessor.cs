namespace Bricks.Images
{
	public interface IImageProcessor
	{
		byte[] Resize(byte[] data, int? width = null, int? height = null, bool preserveAspectRatio = true);
	}
}