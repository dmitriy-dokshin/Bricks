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

			public CultureDisposable(CultureInfo cultureInfo)
			{
				_currentCulture = Thread.CurrentThread.CurrentCulture;
				_cultureInfo = cultureInfo ?? _currentCulture;
				if (!Equals(_currentCulture, _cultureInfo))
				{
					Thread.CurrentThread.CurrentCulture = _cultureInfo;
				}
			}

			#region Implementation of IDisposable

			public void Dispose()
			{
				if (!Equals(_currentCulture, _cultureInfo))
				{
					Thread.CurrentThread.CurrentCulture = _currentCulture;
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

		#endregion
	}
}