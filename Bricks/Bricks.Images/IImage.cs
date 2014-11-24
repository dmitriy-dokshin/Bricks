#region

using System;

#endregion

namespace Bricks.Images
{
	public interface IImage<out TId, out TUserId>
		where TId : struct
		where TUserId : struct
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		TId Id { get; }

		/// <summary>
		/// Данные изображения.
		/// </summary>
		byte[] Data { get; }

		/// <summary>
		/// Ширина изображения.
		/// </summary>
		int Width { get; }

		/// <summary>
		/// Высота изображения.
		/// </summary>
		int Height { get; }

		/// <summary>
		/// Формат изображения.
		/// </summary>
		string Format { get; }

		/// <summary>
		/// Дата создания изображения.
		/// </summary>
		DateTimeOffset CreatedAt { get; }

		/// <summary>
		/// Идентификатор пользователя, который загрузил изображение.
		/// </summary>
		TUserId UserId { get; }
	}
}