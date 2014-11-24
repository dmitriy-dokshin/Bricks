namespace Bricks.Images
{
	public interface IImageProcessor
	{
		byte[] Resize(byte[] data, int width, int height, bool preserveAspectRatio = true);
	}
}