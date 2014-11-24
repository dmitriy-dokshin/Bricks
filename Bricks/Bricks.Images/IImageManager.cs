#region

using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Images
{
	public interface IImageManager<TId, TUserId>
		where TId : struct
		where TUserId : struct
	{
		Task<IResult<IImage<TId, TUserId>>> SaveImage(byte[] data, TUserId userId);

		Task<IResult<IImage<TId, TUserId>>> GetImage(TId imageId, int? width = null, int? height = null, bool preserveAspectRatio = true);
	}
}