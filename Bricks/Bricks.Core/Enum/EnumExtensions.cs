#region

using System;

#endregion

namespace Bricks.Core.Enum
{
	/// <summary>
	/// Содержит методы расширения для <see cref="System.Enum" />.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Приводит значение <paramref name="enum" /> к базовому типу перечисления.
		/// </summary>
		/// <typeparam name="TEnum">Тип значения перечисления.</typeparam>
		/// <param name="enum">Значение перечисления.</param>
		/// <returns>Значение перечисления базового типа.</returns>
		public static System.Enum ToEnum<TEnum>(this TEnum @enum) where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new InvalidOperationException();
			}

			return (System.Enum)(object)@enum;
		}

		/// <summary>
		/// Приводит базовый тип перечисления к типу <typeparamref name="TEnum" />.
		/// </summary>
		/// <typeparam name="TEnum">Целевой тип перечисления.</typeparam>
		/// <param name="enum">Значение перечисления базового типа.</param>
		/// <returns>Значение перечисления типа <typeparamref name="TEnum" />.</returns>
		public static TEnum ToTEnum<TEnum>(this System.Enum @enum) where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new InvalidOperationException();
			}

			return (TEnum)(object)@enum;
		}

		/// <summary>
		/// Приводит значение перечисления к базовому числовому типу.
		/// </summary>
		/// <typeparam name="TEnum">Тип значения перечисления.</typeparam>
		/// <param name="enum">Значение перечисления.</param>
		/// <returns>Числовое значение перечисления.</returns>
		public static int ToInt<TEnum>(this TEnum @enum) where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new InvalidOperationException();
			}

			return (int)(object)@enum;
		}
	}
}