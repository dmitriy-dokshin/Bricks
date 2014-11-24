#region

using System;

#endregion

namespace Bricks.Images
{
	public interface IImageSettings
	{
		int ImageMaxSize { get; }

		int ImagesCountEachLongPeriod { get; }

		TimeSpan ImageLongUploadPeriod { get; }

		TimeSpan CacheLifetime { get; }
	}
}