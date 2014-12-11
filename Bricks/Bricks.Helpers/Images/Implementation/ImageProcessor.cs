#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

namespace Bricks.Helpers.Images.Implementation
{
	internal sealed class ImageProcessor : IImageProcessor
	{
		#region Implementation of IImageProcessor

		public byte[] Resize(byte[] data, int? width = null, int? height = null, bool preserveAspectRatio = true)
		{
			if (!width.HasValue && !height.HasValue)
			{
				return data;
			}

			var getImageResult = data.GetImage();
			if (!getImageResult.Success)
			{
				return data;
			}

			Image image = getImageResult.Data;
			Image newImage = Resize(image, width, height, preserveAspectRatio);
			return newImage.GetBytes();
		}

		public Image Resize(Image image, int? width = null, int? height = null, bool preserveAspectRatio = true)
		{
			if (!width.HasValue && !height.HasValue)
			{
				return image;
			}

			int maxWidth = image.Width * 2;
			if (width.HasValue && width.Value > maxWidth)
			{
				width = maxWidth;
			}

			int maxHeight = image.Height * 2;
			if (height.HasValue && height.Value > maxHeight)
			{
				height = maxHeight;
			}

			Size newSize = GetNewSize(image, width ?? image.Width, height ?? image.Height, preserveAspectRatio);
			Image newImage = new Bitmap(newSize.Width, newSize.Height);
			using (Graphics graphicsHandle = Graphics.FromImage(newImage))
			{
				graphicsHandle.InterpolationMode = InterpolationMode.Bilinear;
				graphicsHandle.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
			}

			return newImage;
		}

		#endregion

		private static Size GetNewSize(Image image, int width, int height, bool preserveAspectRatio)
		{
			Size newSize;
			if (preserveAspectRatio)
			{
				int originalWidth = image.Width;
				int originalHeight = image.Height;
				double percentWidth = width / (double)originalWidth;
				double percentHeight = height / (double)originalHeight;
				double percent = percentHeight < percentWidth ? percentHeight : percentWidth;
				newSize = new Size((int)(Math.Round(originalWidth * percent)), (int)(Math.Round(originalHeight * percent)));
			}
			else
			{
				newSize = new Size(width, height);
			}

			return newSize;
		}
	}
}