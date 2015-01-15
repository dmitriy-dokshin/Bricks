#region

using System.Threading;

using Bricks.Core.Tasks;

#endregion

namespace Bricks.Core.Impl.Tasks
{
	/// <summary>
	/// Провайдер пустого токена отмены.
	/// </summary>
	internal sealed class NoneCancellationTokenProvider : ICancellationTokenProvider
	{
		#region Implementation of ICancellationTokenProvider

		/// <summary>
		/// Получает токен отмены.
		/// </summary>
		/// <returns>Токен отмены.</returns>
		public CancellationToken GetCancellationToken()
		{
			return CancellationToken.None;
		}

		#endregion
	}
}