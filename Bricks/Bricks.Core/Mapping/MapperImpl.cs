#region

using System;
using System.Linq.Expressions;

using AutoMapper;

#endregion

namespace Bricks.Core.Mapping
{
	/// <summary>
	/// ���������� �� ��������� ��� <see cref="IMapper" />.
	/// </summary>
	internal sealed class MapperImpl : IMapper
	{
		#region Implementation of IMapper

		/// <summary>
		/// ��������� ������� ��������� ������� <see cref="source" /> � ������ ���� <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TSource">��� ��������� �������.</typeparam>
		/// <typeparam name="TDestination">��� �������� �������.</typeparam>
		/// <param name="source">�������� ������.</param>
		/// <returns>����� ������ ���� <typeparamref name="TDestination" />.</returns>
		public TDestination DynamicMap<TSource, TDestination>(TSource source)
		{
			return Mapper.DynamicMap<TSource, TDestination>(source);
		}

		/// <summary>
		/// ��������� ������� ��������� ������� <see cref="source" /> � ������ ���� <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">��� �������� �������.</typeparam>
		/// <param name="source">�������� ������.</param>
		/// <returns>����� ������ ���� <typeparamref name="TDestination" />.</returns>
		public object DynamicMap<TDestination>(object source)
		{
			if (source == null)
			{
				return default(TDestination);
			}

			return Mapper.DynamicMap(source, source.GetType(), typeof(TDestination));
		}

		/// <summary>
		/// ��������� ������� ��������� ������� <see cref="source" /> � ������ <paramref name="destination" />.
		/// </summary>
		/// <typeparam name="TSource">��� ��������� �������.</typeparam>
		/// <typeparam name="TDestination">��� �������� �������.</typeparam>
		/// <param name="source">�������� ������.</param>
		/// <param name="destination">������� ������.</param>
		public void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
		{
			Mapper.DynamicMap(source, destination);
		}

		/// <summary>
		/// ��������� ���� �������� ����, ������� ���������� ������������ ��� ��������.
		/// </summary>
		/// <typeparam name="TSource">�������� ���.</typeparam>
		/// <typeparam name="TDestination">������� ���.</typeparam>
		/// <param name="destinationMember">�������, ����������� ���� �������� ����, ������� ����� ������������ ��� ��������.</param>
		public void Ignore<TSource, TDestination>(Expression<Func<TDestination, object>> destinationMember)
		{
			Mapper.CreateMap<TSource, TDestination>().ForMember(destinationMember, x => x.Ignore());
		}

		/// <summary>
		/// ��������� �������, ������� ���������� ������������ ��� �������� ���������������� ����� �������� ����.
		/// </summary>
		/// <typeparam name="TSource">�������� ���.</typeparam>
		/// <typeparam name="TDestination">������� ���.</typeparam>
		/// <param name="destinationMember">
		/// �������, ����������� ���� �������� ����, ��� �������� ����� ����������� �������
		/// ��������.
		/// </param>
		/// <param name="resolver">�������, ������������ ��� �������� ���������������� ����� �������� ����.</param>
		public void ResolveUsing<TSource, TDestination>(Expression<Func<TDestination, object>> destinationMember, Func<TSource, object> resolver)
		{
			Mapper.CreateMap<TSource, TDestination>().ForMember(destinationMember, x => x.ResolveUsing(resolver));
		}

		#endregion
	}
}