#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Bricks.DAL.Fakes.Queryable
{
	/// <summary>
	/// Имитация <see cref="IQueryable{T}" />, подерживающая асинхронные операции.
	/// </summary>
	/// <typeparam name="T">Тип элементов.</typeparam>
	public class FakeQueryable<T> : IQueryable<T>
	{
		private readonly IQueryable<T> _queryable;
		private readonly AsyncQueryProvider<T> _queryProvider;
		private readonly IReadOnlyCollection<T> _source;

		public FakeQueryable(IReadOnlyCollection<T> source = null)
		{
			_source = source ?? new T[0];
			_queryable = _source.AsQueryable();
			_queryProvider = new AsyncQueryProvider<T>(_queryable.Provider);
		}

		public FakeQueryable(params T[] items)
			: this((IReadOnlyCollection<T>)items)
		{
		}

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return _source.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of IQueryable

		/// <summary>
		/// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of
		/// <see cref="T:System.Linq.IQueryable" />.
		/// </returns>
		public Expression Expression
		{
			get
			{
				return _queryable.Expression;
			}
		}

		/// <summary>
		/// Gets the type of the element(s) that are returned when the expression tree associated with this instance of
		/// <see cref="T:System.Linq.IQueryable" /> is executed.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree
		/// associated with this object is executed.
		/// </returns>
		public Type ElementType
		{
			get
			{
				return typeof(T);
			}
		}

		/// <summary>
		/// Gets the query provider that is associated with this data source.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.
		/// </returns>
		public IQueryProvider Provider
		{
			get
			{
				return _queryProvider;
			}
		}

		#endregion
	}
}