#region

using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Configuration;
using Bricks.Core.DateTime;
using Bricks.Core.Extensions;
using Bricks.Core.Results;
using Bricks.Core.Tasks;
using Bricks.Helpers.Reflection;
using Bricks.Helpers.Sync;
using Bricks.Sync;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Images.Implementation
{
	public class ImageManager<TId, TUserId> : IImageManager<TId, TUserId>
		where TId : struct
		where TUserId : struct
	{
		private const string ImageCacheManagerName = "ImagesCacheManager";
		private const string ImageSettingsKey = "imageSettings";
		private const string CacheKeyFormat = "{0}_{1}_{2}_{3}";

		private readonly ICacheManager _cacheManager;
		private readonly CancellationToken _cancellationToken;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IImageProcessor _imageProcessor;
		private readonly IImageSettings _imageSettings;
		private readonly ILockStorage _lockStorage;
		private readonly IReflectionHelper _reflectionHelper;
		private readonly IResultFactory _resultFactory;
		private readonly IServiceLocator _serviceLocator;

		public ImageManager(ILockStorage lockStorage, IConfigurationManager configurationManager, IServiceLocator serviceLocator, IResultFactory resultFactory, IDateTimeProvider dateTimeProvider, IReflectionHelper reflectionHelper, IImageProcessor imageProcessor, ICancellationTokenProvider cancellationTokenProvider)
		{
			_lockStorage = lockStorage;
			_serviceLocator = serviceLocator;
			_resultFactory = resultFactory;
			_dateTimeProvider = dateTimeProvider;
			_reflectionHelper = reflectionHelper;
			_imageProcessor = imageProcessor;
			_cacheManager = CacheFactory.GetCacheManager(ImageCacheManagerName);
			_imageSettings = configurationManager.GetSettings<IImageSettings>(ImageSettingsKey);
			_cancellationToken = cancellationTokenProvider.GetCancellationToken();
		}

		#region Implementation of IImageManager<TImage,in TId,TUserId>

		public async Task<IResult<IImage<TId, TUserId>>> SaveImage(byte[] data, TUserId userId)
		{
			if (data.Length > _imageSettings.ImageMaxSize * 1024)
			{
				string message = string.Format(CultureInfo.InvariantCulture, Resources.ImageManager_SaveImage_ImageMaxSize, _imageSettings.ImageMaxSize);
				return _resultFactory.CreateUnsuccessfulResult<IImage<TId, TUserId>>(message);
			}

			ILockContainer lockContainer;
			using (_lockStorage.GetContainer(_reflectionHelper.GetFullName(MethodBase.GetCurrentMethod()), out lockContainer))
			{
				ILockAsync @lock;
				using (lockContainer.GetLock(userId, out @lock))
				{
					using (await @lock.Enter(_cancellationToken))
					{
						IImageRepository<TId, TUserId> imageRepository = GetImageRepository();
						DateTimeOffset now = _dateTimeProvider.Now;
						DateTimeOffset createdAtFrom = now - _imageSettings.ImageLongUploadPeriod;
						IResult<Tuple<int, DateTimeOffset>> getUserImagesCountResult = await imageRepository.GetUserImagesCount(userId, createdAtFrom);
						if (!getUserImagesCountResult.Success)
						{
							return _resultFactory.CreateUnsuccessfulResult<IImage<TId, TUserId>>(innerResult: getUserImagesCountResult);
						}

						if (getUserImagesCountResult.Data.Item1 + 1 > _imageSettings.ImagesCountEachLongPeriod)
						{
							TimeSpan timeout = getUserImagesCountResult.Data.Item2 + _imageSettings.ImageLongUploadPeriod - now;
							string message = string.Format(
								CultureInfo.InvariantCulture, Resources.ImageManager_SaveImage_ImageMaxCount,
								_imageSettings.ImagesCountEachLongPeriod, timeout.ToDetailedString());
							return _resultFactory.CreateUnsuccessfulResult<IImage<TId, TUserId>>(message);
						}

						try
						{
							Image image = data.GetImage();
							IResult<IImage<TId, TUserId>> saveImageResult = await imageRepository.SaveImage(new Image<TId, TUserId>(image, now, userId));
							if (!saveImageResult.Success)
							{
								return _resultFactory.CreateUnsuccessfulResult<IImage<TId, TUserId>>(innerResult: saveImageResult);
							}

							return _resultFactory.Create(saveImageResult.Data);
						}
						catch (ArgumentException exception)
						{
							return _resultFactory.CreateUnsuccessfulResult<IImage<TId, TUserId>>(exception: exception);
						}
					}
				}
			}
		}

		public async Task<IResult<IImage<TId, TUserId>>> GetImage(TId imageId, int? width = null, int? height = null, bool preserveAspectRatio = true)
		{
			string key = string.Format(CultureInfo.InvariantCulture, CacheKeyFormat, imageId, width, height, preserveAspectRatio);

			var image = (IImage<TId, TUserId>)_cacheManager.GetData(key);
			if (image == null)
			{
				ILockContainer lockContainer;
				using (_lockStorage.GetContainer(_reflectionHelper.GetFullName(MethodBase.GetCurrentMethod()), out lockContainer))
				{
					ILockAsync @lock;
					using (lockContainer.GetLock(imageId, out @lock))
					{
						using (await @lock.Enter(_cancellationToken))
						{
							image = (IImage<TId, TUserId>)_cacheManager.GetData(key);
							if (image == null)
							{
								IImageRepository<TId, TUserId> imageRepository = GetImageRepository();
								IResult<IImage<TId, TUserId>> getImageResult =
									await (!width.HasValue && !height.HasValue
											   ? imageRepository.GetImage(imageId)
											   : GetImage(imageId));
								if (getImageResult.Success)
								{
									image = getImageResult.Data;
									if (width.HasValue && image.Width != width.Value || height.HasValue && image.Height != height.Value)
									{
										// Необходимо изменить размер.
										byte[] data = _imageProcessor.Resize(
											image.Data, width ?? image.Width, height ?? image.Height, preserveAspectRatio);
										image = new Image<TId, TUserId>(data.GetImage(), _dateTimeProvider.Now, image.UserId);
									}

									_cacheManager.Add(key, image, CacheItemPriority.Normal, null, new AbsoluteTime(_imageSettings.CacheLifetime));
								}
								else
								{
									return _resultFactory.CreateUnsuccessfulResult<IImage<TId, TUserId>>(innerResult: getImageResult);
								}
							}
						}
					}
				}
			}

			return _resultFactory.Create(image);
		}

		#endregion

		private IImageRepository<TId, TUserId> GetImageRepository()
		{
			return _serviceLocator.GetInstance<IImageRepository<TId, TUserId>>();
		}
	}
}