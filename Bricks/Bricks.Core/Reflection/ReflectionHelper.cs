#region

using System;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Bricks.Core.Reflection
{
	/// <summary>
	/// Помощник работы с рефлексией.
	/// </summary>
	public static class ReflectionHelper
	{
		/// <summary>
		/// Получает название члена класса на основе выражения <paramref name="expression" />.
		/// </summary>
		/// <typeparam name="T">Тип возвращаемого значения.</typeparam>
		/// <param name="expression">Выражение, являющееся обращением к члену класса.</param>
		/// <returns>Название члена классса.</returns>
		public static string GetMemberName<T>(this Expression<Func<T>> expression)
		{
			var memberExpression = (MemberExpression)expression.Body;
			return memberExpression.Member.Name;
		}

		/// <summary>
		/// Получает название переменной из выражения <paramref name="expression" />.
		/// </summary>
		/// <typeparam name="T">Тип возвращаемого значения.</typeparam>
		/// <param name="expression">Выражение, являющееся обращением к переменной.</param>
		/// <returns>Название переменной.</returns>
		public static string GetVariableName<T>(this Expression<Func<T>> expression)
		{
			var runtimeVariablesExpression = (RuntimeVariablesExpression)expression.Body;
			return runtimeVariablesExpression.Variables.First().Name;
		}
	}
}