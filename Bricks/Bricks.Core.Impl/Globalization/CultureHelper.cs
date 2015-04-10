#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Collections;
using Bricks.Core.Globalization;

#endregion

namespace Bricks.Core.Impl.Globalization
{
	internal sealed class CultureHelper : ICultureHelper
	{
		private readonly ICollectionHelper _collectionHelper;
		private readonly IReadOnlyDictionary<CultureInfo, IReadOnlyCollection<CultureInfo>> _cultureInfosByParentCulture;

		public CultureHelper(ICollectionHelper collectionHelper)
		{
			_cultureInfosByParentCulture =
				CultureInfo.GetCultures(CultureTypes.AllCultures)
					.GroupBy(x => x.Parent)
					.ToDictionary(x => x.Key, x => (IReadOnlyCollection<CultureInfo>)x.ToArray());
			_collectionHelper = collectionHelper;
		}

		private sealed class CultureDisposable : IDisposable
		{
			private readonly CultureInfo _cultureInfo;
			private readonly CultureInfo _currentCulture;
			private readonly CultureInfo _currentUICulture;

			public CultureDisposable(CultureInfo cultureInfo)
			{
				_cultureInfo = cultureInfo;
				if (cultureInfo != null)
				{
					Thread currentThread = Thread.CurrentThread;
					_currentCulture = currentThread.CurrentCulture;
					_currentUICulture = currentThread.CurrentUICulture;
					currentThread.CurrentCulture = cultureInfo;
					currentThread.CurrentUICulture = cultureInfo;
				}
			}

			#region Implementation of IDisposable

			public void Dispose()
			{
				if (_cultureInfo != null)
				{
					Thread currentThread = Thread.CurrentThread;
					currentThread.CurrentCulture = _currentCulture;
					currentThread.CurrentUICulture = _currentUICulture;
				}
			}

			#endregion
		}

		#region Implementation of ICultureHelper

		public IReadOnlyCollection<CultureInfo> GetCultures(CultureInfo parentCulture)
		{
			IReadOnlyCollection<CultureInfo> cultureInfos;
			if (!_cultureInfosByParentCulture.TryGetValue(parentCulture, out cultureInfos))
			{
				cultureInfos = _collectionHelper.GetEmptyReadOnlyCollection<CultureInfo>();
			}

			return cultureInfos;
		}

		public void Execute(Action action, CultureInfo cultureInfo)
		{
			using (new CultureDisposable(cultureInfo))
			{
				action();
			}
		}

		public async Task ExecuteAsync(Func<Task> action, CultureInfo cultureInfo)
		{
			using (new CultureDisposable(cultureInfo))
			{
				await action();
			}
		}

		public TResult Execute<TResult>(Func<TResult> func, CultureInfo cultureInfo)
		{
			using (new CultureDisposable(cultureInfo))
			{
				return func();
			}
		}

		public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, CultureInfo cultureInfo)
		{
			using (new CultureDisposable(cultureInfo))
			{
				return await func();
			}
		}

		public IDisposable UseCulture(CultureInfo cultureInfo)
		{
			return new CultureDisposable(cultureInfo);
		}

		#endregion
	}
}