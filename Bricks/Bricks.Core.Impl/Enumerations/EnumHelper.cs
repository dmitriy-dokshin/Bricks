#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Bricks.Core.Collections;
using Bricks.Core.Enumerations;

#endregion

namespace Bricks.Core.Impl.Enumerations
{
	/// <summary>
	/// Помощник работы с перечислениями.
	/// </summary>
	internal sealed class EnumHelper : IEnumHelper
	{
		private static readonly char[] _arrayParamSeparators = { ' ', ',' };
		private readonly ICollectionHelper _collectionHelper;

		public EnumHelper(ICollectionHelper collectionHelper)
		{
			_collectionHelper = collectionHelper;
		}

		/// <summary>
		/// Возвращает признак того, что перечисление типа <paramref name="enumType" /> является флаговым.
		/// </summary>
		/// <param name="enumType">Тип перечисления.</param>
		/// <returns>Признак флагового перечисления.</returns>
		public bool IsFlags(Type enumType)
		{
			bool isFlags = enumType.GetCustomAttribute<FlagsAttribute>(false) != null;
			return isFlags;
		}

		/// <summary>
		/// Приводит строку с элементами перечисления <typeparamref name="TEnum" />
		/// к коллекции с элементами перечисления <typeparamref name="TEnum" />.
		/// </summary>
		/// <typeparam name="TEnum">Тип элементов перечисления.</typeparam>
		/// <param name="source">
		/// Строка с элементами перечисления <typeparamref name="TEnum" />.
		/// </param>
		/// <returns>Коллекция с элементами перечисления <typeparamref name="TEnum" />.</returns>
		public IReadOnlyCollection<TEnum> ToEnumCollection<TEnum>(string source) where TEnum : struct
		{
			Type enumType = typeof(TEnum);
			if (!enumType.IsEnum)
			{
				throw new InvalidOperationException();
			}

			if (string.IsNullOrEmpty(source))
			{
				return _collectionHelper.GetEmptyReadOnlyCollection<TEnum>();
			}

			HashSet<Enum> flags = null;
			bool isFlags = IsFlags(enumType);
			if (isFlags)
			{
				flags = new HashSet<Enum>(Enum.GetValues(enumType).Cast<Enum>());
				flags = new HashSet<Enum>(flags.Where(x => flags.Count(x.HasFlag) == 1));
			}

			var values = new HashSet<TEnum>();
			foreach (string property in source.Split(_arrayParamSeparators, StringSplitOptions.RemoveEmptyEntries))
			{
				TEnum value;
				if (Enum.TryParse(property, true, out value))
				{
					Enum @enum = value.ToEnum();
					if (isFlags)
					{
						foreach (Enum flag in flags.Where(@enum.HasFlag))
						{
							values.Add(flag.ToTEnum<TEnum>());
						}
					}
					else
					{
						values.Add(value);
					}
				}
			}

			return values.ToArray();
		}
	}
}