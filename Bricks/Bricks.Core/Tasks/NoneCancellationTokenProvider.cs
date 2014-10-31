#region

using System.Threading;

#endregion

namespace Bricks.Core.Tasks
{
	/// <summary>
	/// Провайдер пустого токена отмены.
	/// </summary>
	public sealed class NoneCancellationTokenProvider : ICancellationTokenProvider
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