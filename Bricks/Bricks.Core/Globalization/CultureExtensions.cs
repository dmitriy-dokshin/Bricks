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

		public static CultureInfo GetParentCulture(this CultureInfo cultureInfo)
		{
			if (!Equals(cultureInfo.Parent, CultureInfo.InvariantCulture))
			{
				return cultureInfo.Parent.GetParentCulture();
			}

			return cultureInfo;
		}
	}
}