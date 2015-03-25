#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Bricks.Core.Enumerations
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
		public static Enum ToEnum<TEnum>(this TEnum @enum) where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new InvalidOperationException();
			}

			return (Enum)(object)@enum;
		}

		/// <summary>
		/// Приводит базовый тип перечисления к типу <typeparamref name="TEnum" />.
		/// </summary>
		/// <typeparam name="TEnum">Целевой тип перечисления.</typeparam>
		/// <param name="enum">Значение перечисления базового типа.</param>
		/// <returns>Значение перечисления типа <typeparamref name="TEnum" />.</returns>
		public static TEnum ToTEnum<TEnum>(this Enum @enum) where TEnum : struct
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

		public static IReadOnlyCollection<TEnum> GetFlags<TEnum>(this TEnum source) where TEnum : struct
		{
			Enum @enum = source.ToEnum();
			IReadOnlyCollection<TEnum> flags = GetFlags<TEnum>(@enum);
			return flags;
		}

		public static IReadOnlyCollection<TEnum> GetFlags<TEnum>(this Enum @enum) where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new InvalidOperationException();
			}

			IReadOnlyCollection<TEnum> flags = GetFlags(@enum).Cast<TEnum>().ToArray();
			return flags;
		}

		public static IReadOnlyCollection<Enum> GetFlags(this Enum @enum)
		{
			Type type = @enum.GetType();
			if (type.GetCustomAttribute(typeof(FlagsAttribute), false) == null)
			{
				throw new InvalidOperationException();
			}

			IEnumerable<Enum> values = Enum.GetValues(type).Cast<Enum>();
			IReadOnlyCollection<Enum> flags = values.Where(@enum.HasFlag).ToArray();
			return flags;
		}

		public static object GetValue(this Enum @enum)
		{
			return Convert.ChangeType(@enum, Enum.GetUnderlyingType(@enum.GetType()));
		}
	}
}