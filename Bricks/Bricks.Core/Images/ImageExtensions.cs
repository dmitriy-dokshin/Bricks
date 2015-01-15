#region

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using Bricks.Core.Exceptions;
using Bricks.Core.Results;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Images
{
	public static class ImageExtensions
	{
		private static readonly Lazy<IExceptionHelper> _exceptionHelper;

		static ImageExtensions()
		{
			_exceptionHelper = new Lazy<IExceptionHelper>(
				ServiceLocator.Current.GetInstance<IExceptionHelper>, true);
		}

		public static IResult<Image> GetImage(this byte[] data)
		{
			using (var memoryStream = new MemoryStream(data))
			{
				return memoryStream.GetImage();
			}
		}

		public static IResult<Image> GetImage(this Stream stream)
		{
			return _exceptionHelper.Value.Catch<Image, ArgumentException>(() => Image.FromStream(stream));
		}

		public static byte[] GetBytes(this Image image)
		{
			using (var memoryStream = new MemoryStream())
			{
				image.SaveFixed(memoryStream, image.RawFormat);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Сохраняет изображение <paramref name="image" /> в поток <paramref name="stream" />
		/// в формате <paramref name="format" />.
		/// </summary>
		/// <param name="image">Изображение.</param>
		/// <param name="stream">Поток для сохранения.</param>
		/// <param name="format">Формат изображения.</param>
		/// <remarks>
		/// Информацию об исправлении смотри:
		/// http://stackoverflow.com/questions/9073619/image-save-crashing-value-cannot-be-null-r-nparameter-name-encoder
		/// </remarks>
		public static void SaveFixed(this Image image, Stream stream, ImageFormat format)
		{
			if (format.Equals(ImageFormat.MemoryBmp))
			{
				format = ImageFormat.Bmp;
			}

			try
			{
				image.Save(stream, format);
			}
			catch (ExternalException)
			{
				// См. http://social.msdn.microsoft.com/Forums/vstudio/en-US/b15357f1-ad9d-4c80-9ec1-92c786cca4e6/bitmapsave-a-generic-error-occurred-in-gdi
				image.SaveAsBitmap(stream, format);
			}
			catch (AccessViolationException)
			{
				image.SaveAsBitmap(stream, format);
			}
		}

		/// <summary>
		/// Сохраняет изображение, предварительно приводя его к <see cref="Bitmap" />.
		/// </summary>
		/// <param name="image">Изображение.</param>
		/// <param name="stream">Поток для сохранения.</param>
		/// <param name="format">Формат изображения.</param>
		public static void SaveAsBitmap(this Image image, Stream stream, ImageFormat format)
		{
			var bitmap = image as Bitmap;
			if (bitmap != null)
			{
				image = new Bitmap(bitmap);
			}

			image.Save(stream, format);
		}

		/// <summary>
		/// Возвращает строку, соответствующую формату изображения <paramref name="imageFormat" />.
		/// </summary>
		/// <param name="imageFormat">Формат изображения.</param>
		/// <returns>Строка соответствующая формату изображения.</returns>
		/// <remarks>
		/// Информацию об исправленной ошибке смотри:
		/// https://connect.microsoft.com/VisualStudio/feedback/details/845070/reference-comparison-is-used-in-system-drawing-imaging-imageformat-tostring
		/// </remarks>
		public static string ToStringFixed(this ImageFormat imageFormat)
		{
			if (imageFormat.Equals(ImageFormat.Bmp) || imageFormat.Equals(ImageFormat.MemoryBmp))
			{
				// Смотри: https://connect.microsoft.com/VisualStudio/feedback/details/845070/reference-comparison-is-used-in-system-drawing-imaging-imageformat-tostring
				return "Bmp";
			}

			if (imageFormat.Equals(ImageFormat.Emf))
			{
				return "Emf";
			}

			if (imageFormat.Equals(ImageFormat.Wmf))
			{
				return "Wmf";
			}

			if (imageFormat.Equals(ImageFormat.Gif))
			{
				return "Gif";
			}

			if (imageFormat.Equals(ImageFormat.Jpeg))
			{
				return "Jpeg";
			}

			if (imageFormat.Equals(ImageFormat.Png))
			{
				return "Png";
			}

			if (imageFormat.Equals(ImageFormat.Tiff))
			{
				return "Tiff";
			}

			if (imageFormat.Equals(ImageFormat.Exif))
			{
				return "Exif";
			}

			if (imageFormat.Equals(ImageFormat.Icon))
			{
				return "Icon";
			}

			return "[ImageFormat: " + imageFormat.Guid + "]";
		}
	}
}