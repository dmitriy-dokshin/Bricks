#region

using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.SMS
{
	/// <summary>
	/// Сервис отправки SMS.
	/// </summary>
	public interface ISmsService
	{
		/// <summary>
		/// Отправляет SMS с текстом <paramref name="text" /> на номер телефона <paramref name="phoneNumber" />.
		/// </summary>
		/// <param name="phoneNumber">Номер телефона.</param>
		/// <param name="text">Текст сообщения.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns />
		Task SendAsync(string phoneNumber, string text, CancellationToken cancellationToken);
	}
}