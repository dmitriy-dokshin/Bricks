#region

using System;
using System.Configuration;

#endregion

namespace Bricks.Images.Implementation
{
	internal sealed class ImageSettings : ConfigurationSection, IImageSettings
	{
		private const string ImageMaxSizeName = "imageMaxSize";
		private const string ImagesCountEachLongPeriodName = "imagesCountEachLongPeriod";
		private const string ImageLongUploadPeriodName = "imageLongUploadPeriod";
		private const string CacheLifetimeName = "cacheLifetime";

		#region Implementation of IImageSettings

		[ConfigurationProperty(ImageLongUploadPeriodName)]
		public int ImageLongUploadPeriod
		{
			get
			{
				return (int)this[ImageLongUploadPeriodName];
			}
		}

		[ConfigurationProperty(CacheLifetimeName)]
		public int CacheLifetime
		{
			get
			{
				return (int)this[CacheLifetimeName];
			}
		}

		TimeSpan IImageSettings.CacheLifetime
		{
			get
			{
				return TimeSpan.FromMinutes(CacheLifetime);
			}
		}

		[ConfigurationProperty(ImageMaxSizeName)]
		public int ImageMaxSize
		{
			get
			{
				return (int)this[ImageMaxSizeName];
			}
		}

		[ConfigurationProperty(ImagesCountEachLongPeriodName)]
		public int ImagesCountEachLongPeriod
		{
			get
			{
				return (int)this[ImagesCountEachLongPeriodName];
			}
		}

		TimeSpan IImageSettings.ImageLongUploadPeriod
		{
			get
			{
				return TimeSpan.FromHours(ImageLongUploadPeriod);
			}
		}

		#endregion
	}
}