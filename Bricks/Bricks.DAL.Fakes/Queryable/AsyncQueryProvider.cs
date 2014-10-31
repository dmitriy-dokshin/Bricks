#region

using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Bricks.DAL.Fakes.Queryable
{
	public class AsyncQueryProvider<T> : IQueryProvider, IDbAsyncQueryProvider
	{
		private readonly IQueryProvider _queryProvider;

		public AsyncQueryProvider(IQueryProvider queryProvider)
		{
			_queryProvider = queryProvider;
		}

		#region Implementation of IQueryProvider

		public IQueryable CreateQuery(Expression expression)
		{
			return new AsyncEnumerableQuery<T>(expression);
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new AsyncEnumerableQuery<TElement>(expression);
		}

		public object Execute(Expression expression)
		{
			return _queryProvider.Execute(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return _queryProvider.Execute<TResult>(expression);
		}

		#endregion

		#region Implementation of IDbAsyncQueryProvider

		public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Task.FromResult(Execute(expression));
		}

		public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Task.FromResult(Execute<TResult>(expression));
		}

		#endregion
	}
}