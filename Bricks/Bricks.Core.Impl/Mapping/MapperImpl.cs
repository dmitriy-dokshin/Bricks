#region

using System;
using System.Linq.Expressions;

using AutoMapper;

using Bricks.Core.Mapping;

#endregion

namespace Bricks.Core.Impl.Mapping
{
	/// <summary>
	/// Реализация по умолчанию для <see cref="IMapper" />.
	/// </summary>
	internal sealed class MapperImpl : IMapper
	{
		#region Implementation of IMapper

		/// <summary>
		/// Выполняет маппинг исходного объекта <see cref="source" /> в объект типа <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TSource">Тип исходного объекта.</typeparam>
		/// <typeparam name="TDestination">Тип целевого объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект типа <typeparamref name="TDestination" />.</returns>
		public TDestination DynamicMap<TSource, TDestination>(TSource source)
		{
			return Mapper.DynamicMap<TSource, TDestination>(source);
		}

		/// <summary>
		/// Выполняет маппинг исходного объекта <see cref="source" /> в объект типа <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Тип целевого объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Новый объект типа <typeparamref name="TDestination" />.</returns>
		public object DynamicMap<TDestination>(object source)
		{
			if (source == null)
			{
				return default(TDestination);
			}

			return Mapper.DynamicMap(source, source.GetType(), typeof(TDestination));
		}

		/// <summary>
		/// Выполняет маппинг исходного объекта <see cref="source" /> в объект <paramref name="destination" />.
		/// </summary>
		/// <typeparam name="TSource">Тип исходного объекта.</typeparam>
		/// <typeparam name="TDestination">Тип целевого объекта.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <param name="destination">Целевой объект.</param>
		public void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
		{
			Mapper.DynamicMap(source, destination);
		}

		/// <summary>
		/// Указывает член целевого типа, который необходимо игнорировать при маппинге.
		/// </summary>
		/// <typeparam name="TSource">Исходный тип.</typeparam>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="destinationMember">Функция, указывающая член целевого типа, который нужно игнорировать при маппинге.</param>
		public void Ignore<TSource, TDestination>(Expression<Func<TDestination, object>> destinationMember)
		{
			Mapper.CreateMap<TSource, TDestination>().ForMember(destinationMember, x => x.Ignore());
		}

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
		public void ResolveUsing<TSource, TDestination>(Expression<Func<TDestination, object>> destinationMember, Func<TSource, object> resolver)
		{
			Mapper.CreateMap<TSource, TDestination>().ForMember(destinationMember, x => x.ResolveUsing(resolver));
		}

		/// <summary>
		/// Skip member mapping and use a custom function to convert to the destination type
		/// </summary>
		/// <param name="mappingFunction">Callback to convert from source type to destination type</param>
		public void ConvertUsing<TSource, TDestination>(Func<TSource, TDestination> mappingFunction)
		{
			Mapper.CreateMap<TSource, TDestination>().ConvertUsing(mappingFunction);
		}

		#endregion
	}
}