#region

using System;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

#endregion

namespace Bricks.Core.Images
{
	public static class ImageHelper
	{
		public static ImageFormat ParseFormat(string format)
		{
			format = format.ToLowerInvariant();
			if (string.Equals(format, "Bmp", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Bmp;
			}

			if (string.Equals(format, "Emf", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Emf;
			}

			if (string.Equals(format, "Wmf", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Wmf;
			}

			if (string.Equals(format, "Gif", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Gif;
			}

			if (string.Equals(format, "Jpeg", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Jpeg;
			}

			if (string.Equals(format, "Png", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Png;
			}

			if (string.Equals(format, "Tiff", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Tiff;
			}

			if (string.Equals(format, "Exif", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Exif;
			}

			if (string.Equals(format, "Icon", StringComparison.OrdinalIgnoreCase))
			{
				return ImageFormat.Icon;
			}

			Match match = System.Text.RegularExpressions.Regex.Match(format, @"\[ImageFormat: (?<guid>[a-zA-Z0-9-]+)\]", RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				throw new NotSupportedException();
			}

			Guid guid = Guid.Parse(match.Groups["guid"].Value);
			return new ImageFormat(guid);
		}
	}
}