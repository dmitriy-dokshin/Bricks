#region

using System;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Disposing
{
	public static class DisposableExtensions
	{
		private static readonly IDisposableHelper _disposableHelper;

		static DisposableExtensions()
		{
			_disposableHelper = ServiceLocator.Current.GetInstance<IDisposableHelper>();
		}

		public static IDisposable After(this IDisposable target, Action action)
		{
			return _disposableHelper.Action(() =>
			{
				target.Dispose();
				action();
			});
		}

		public static IDisposable Before(this IDisposable target, Action action)
		{
			return _disposableHelper.Action(() =>
			{
				action();
				target.Dispose();
			});
		}
	}
}