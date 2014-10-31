#region

using System.Threading;

#endregion

namespace Bricks.Core.Tasks
{
	/// <summary>
	/// Поставщик токена отмена.
	/// </summary>
	public interface ICancellationTokenProvider
	{
		/// <summary>
		/// Получает токен отмены.
		/// </summary>
		/// <returns>Токен отмены.</returns>
		CancellationToken GetCancellationToken();
	}
}