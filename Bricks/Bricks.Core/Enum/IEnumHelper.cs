#region

using System;
using System.Collections.Generic;

#endregion

namespace Bricks.Core.Enum
{
	/// <summary>
	/// Помощник работы с перечислениями.
	/// </summary>
	public interface IEnumHelper
	{
		/// <summary>
		/// Возвращает признак того, что перечисление типа <paramref name="enumType" /> является флаговым.
		/// </summary>
		/// <param name="enumType">Тип перечисления.</param>
		/// <returns>Признак флагового перечисления.</returns>
		bool IsFlags(Type enumType);

		/// <summary>
		/// Приводит строку с элементами перечисления <typeparamref name="TEnum" />
		/// к коллекции с элементами перечисления <typeparamref name="TEnum" />.
		/// </summary>
		/// <typeparam name="TEnum">Тип перечисления.</typeparam>
		/// <param name="source">
		/// Строка с элементами перечисления <typeparamref name="TEnum" />.
		/// </param>
		/// <returns>Коллекция с элементами перечисления <typeparamref name="TEnum" />.</returns>
		IReadOnlyCollection<TEnum> ToEnumCollection<TEnum>(string source) where TEnum : struct;
	}
}