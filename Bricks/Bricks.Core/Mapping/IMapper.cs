#region

using System;
using System.Linq.Expressions;

#endregion

namespace Bricks.Core.Mapping
{
	/// <summary>
	/// Содержит методы маппинга объектов.
	/// </summary>
	public interface IMapper
	{
		/// <summary>
		/// Выполняет маппинг исходного объекта <see cref="source" /> в объект типа <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TSource">Тип исходного объекта.</typeparam>
		/// <typeparam name="TDestination">Тип целевого объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект типа <typeparamref name="TDestination" />.</returns>
		TDestination DynamicMap<TSource, TDestination>(TSource source);

		/// <summary>
		/// Выполняет маппинг исходного объекта <see cref="source" /> в объект типа <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Тип целевого объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект типа <typeparamref name="TDestination" />.</returns>
		object DynamicMap<TDestination>(object source);

		/// <summary>
		/// Выполняет маппинг исходного объекта <see cref="source" /> в объект <paramref name="destination" />.
		/// </summary>
		/// <typeparam name="TSource">Тип исходного объекта.</typeparam>
		/// <typeparam name="TDestination">Тип целевого объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <param name="destination">Целевой объект.</param>
		void DynamicMap<TSource, TDestination>(TSource source, TDestination destination);

		/// <summary>
		/// Указывает член целевого типа, который необходимо игнорировать при маппинге.
		/// </summary>
		/// <typeparam name="TSource">Исходный тип.</typeparam>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="destinationMember">Функция, указывающая член целевого типа, который нужно игнорировать при маппинге.</param>
		void Ignore<TSource, TDestination>(Expression<Func<TDestination, object>> destinationMember);

		/// <summary>
		/// Указывает функцию, которую необходимо использовать при маппинге соответствующего члена целевого типа.
		/// </summary>
		/// <typeparam name="TSource">Исходный тип.</typeparam>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="destinationMember">
		/// Функция, указывающая член целевого типа, для которого будет применяться функция
		/// маппинга.
		/// </param>
		/// <param name="resolver">Функция, используемая при маппинге соответствующего члена целевого типа.</param>
		void ResolveUsing<TSource, TDestination>(Expression<Func<TDestination, object>> destinationMember, Func<TSource, object> resolver);
	}
}