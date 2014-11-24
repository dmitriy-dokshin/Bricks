#region

using System;
using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Images
{
	public interface IImageRepository<TId, TUserId>
		where TId : struct
		where TUserId : struct
	{
		Task<IResult<IImage<TId, TUserId>>> SaveImage(IImage<TId, TUserId> image);

		Task<IResult<IImage<TId, TUserId>>> GetImage(TId imageId);

		Task<IResult<Tuple<int, DateTimeOffset>>> GetUserImagesCount(TUserId userId, DateTimeOffset? createdAtFrom = null);
	}
}