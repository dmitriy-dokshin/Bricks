#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

namespace Bricks.Images.Implementation
{
	internal sealed class ImageProcessor : IImageProcessor
	{
		#region Implementation of IImageProcessor

		public byte[] Resize(byte[] data, int width, int height, bool preserveAspectRatio = true)
		{
			var size = new Size(width, height);
			Image image = data.GetImage();

			Size newSize = GetNewSize(image, size, preserveAspectRatio);
			Image newImage = new Bitmap(newSize.Width, newSize.Height);
			using (Graphics graphicsHandle = Graphics.FromImage(newImage))
			{
				graphicsHandle.InterpolationMode = InterpolationMode.Bilinear;
				graphicsHandle.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
			}

			return newImage.GetBytes();
		}

		#endregion

		/// <summary>
		/// Рассчитывает новый размер изображения <paramref name="image" />.
		/// </summary>
		/// <param name="image">Исходное изображение.</param>
		/// <param name="size">Размер нового изображения.</param>
		/// <param name="preserveAspectRatio">Признак сохранения пропорций исходного изображения.</param>
		/// <returns>Новый размер изображения.</returns>
		private static Size GetNewSize(Image image, Size size, bool preserveAspectRatio)
		{
			Size newSize;
			if (preserveAspectRatio)
			{
				int originalWidth = image.Width;
				int originalHeight = image.Height;
				double percentWidth = size.Width / (double)originalWidth;
				double percentHeight = size.Height / (double)originalHeight;
				double percent = percentHeight < percentWidth ? percentHeight : percentWidth;
				newSize = new Size((int)(Math.Round(originalWidth * percent)), (int)(Math.Round(originalHeight * percent)));
			}
			else
			{
				newSize = size;
			}

			return newSize;
		}
	}
}