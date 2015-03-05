#region

using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Globalization
{
	public static class CultureExtensions
	{
		private static readonly ICultureHelper _cultureHelper;

		static CultureExtensions()
		{
			_cultureHelper = ServiceLocator.Current.GetInstance<ICultureHelper>();
		}

		public static void Execute(this ICultureProvider cultureProvider, Action action)
		{
			_cultureHelper.Execute(action, cultureProvider.CurrentCulture);
		}

		public static Task ExecuteAsync(this ICultureProvider cultureProvider, Func<Task> action)
		{
			return _cultureHelper.Execute(action, cultureProvider.CurrentCulture);
		}

		public static TResult Execute<TResult>(this ICultureProvider cultureProvider, Func<TResult> func)
		{
			return _cultureHelper.Execute(func, cultureProvider.CurrentCulture);
		}

		public static Task<TResult> ExecuteAsync<TResult>(this ICultureProvider cultureProvider, Func<Task<TResult>> func)
		{
			return _cultureHelper.Execute(func, cultureProvider.CurrentCulture);
		}

		public static int? GetCurrentCultureLCID(this ICultureProvider cultureProvider)
		{
			CultureInfo currentCulture = cultureProvider.CurrentCulture;
			return currentCulture != null ? currentCulture.LCID : (int?)null;
		}

		public static int? GetCurrentUICultureLCID(this ICultureProvider cultureProvider)
		{
			CultureInfo currentUICulture = cultureProvider.CurrentUICulture;
			return currentUICulture != null ? currentUICulture.LCID : (int?)null;
		}
	}
}