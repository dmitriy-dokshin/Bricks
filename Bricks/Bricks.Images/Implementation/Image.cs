#region

using System;
using System.Drawing;

#endregion

namespace Bricks.Images.Implementation
{
	internal sealed class Image<TId, TUserId> : IImage<TId, TUserId>
		where TId : struct
		where TUserId : struct
	{
		private readonly Image _image;

		public Image(Image image, DateTimeOffset createdAt, TUserId userId)
		{
			_image = image;
			CreatedAt = createdAt;
			UserId = userId;
		}

		#region Implementation of IImage<out TId,out TUserId>

		/// <summary>
		/// Дата создания изображения.
		/// </summary>
		public DateTimeOffset CreatedAt { get; private set; }

		/// <summary>
		/// Идентификатор.
		/// </summary>
		public TId Id
		{
			get
			{
				return default(TId);
			}
		}

		/// <summary>
		/// Данные изображения.
		/// </summary>
		public byte[] Data
		{
			get
			{
				return _image.GetBytes();
			}
		}

		/// <summary>
		/// Ширина изображения.
		/// </summary>
		public int Width
		{
			get
			{
				return _image.Width;
			}
		}

		/// <summary>
		/// Высота изображения.
		/// </summary>
		public int Height
		{
			get
			{
				return _image.Height;
			}
		}

		/// <summary>
		/// Формат изображения.
		/// </summary>
		public string Format
		{
			get
			{
				return _image.RawFormat.ToStringFixed();
			}
		}

		/// <summary>
		/// Идентификатор пользователя, который загрузил изображение.
		/// </summary>
		public TUserId UserId { get; private set; }

		#endregion
	}
}