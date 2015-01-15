#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Bricks.Core.Images;

#endregion

namespace Bricks.Core.Impl.Images
{
	internal sealed class ImageProcessor : IImageProcessor
	{
		private static Size GetNewSize(Image image, int width, int height, bool preserveAspectRatio)
		{
			Size newSize;
			if (preserveAspectRatio)
			{
				var originalWidth = image.Width;
				var originalHeight = image.Height;
				var percentWidth = width / (double)originalWidth;
				var percentHeight = height / (double)originalHeight;
				var percent = percentHeight < percentWidth ? percentHeight : percentWidth;
				newSize = new Size((int)(Math.Round(originalWidth * percent)), (int)(Math.Round(originalHeight * percent)));
			}
			else
			{
				newSize = new Size(width, height);
			}

			return newSize;
		}

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

			var image = getImageResult.Data;
			var newImage = Resize(image, width, height, preserveAspectRatio);
			return newImage.GetBytes();
		}

		public Image Resize(Image image, int? width = null, int? height = null, bool preserveAspectRatio = true)
		{
			if (!width.HasValue && !height.HasValue)
			{
				return image;
			}

			var maxWidth = image.Width * 2;
			if (width.HasValue && width.Value > maxWidth)
			{
				width = maxWidth;
			}

			var maxHeight = image.Height * 2;
			if (height.HasValue && height.Value > maxHeight)
			{
				height = maxHeight;
			}

			var newSize = GetNewSize(image, width ?? image.Width, height ?? image.Height, preserveAspectRatio);
			Image newImage = new Bitmap(newSize.Width, newSize.Height);
			using (var graphicsHandle = Graphics.FromImage(newImage))
			{
				graphicsHandle.InterpolationMode = InterpolationMode.Bilinear;
				graphicsHandle.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
			}

			return newImage;
		}

		#endregion
	}
}