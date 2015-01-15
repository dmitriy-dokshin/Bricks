#region

using System.Threading.Tasks;

using Bricks.Core.Results;

#endregion

namespace Bricks.Core.Auth.ExternalLogins
{
	public interface IExternalLoginProvider
	{
		Task<IResult<IExternalLoginData>> GetExternalLoginData(string accessToken);
	}
}