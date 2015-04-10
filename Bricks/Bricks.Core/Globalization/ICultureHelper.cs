#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

#endregion

namespace Bricks.Core.Globalization
{
	public interface ICultureHelper
	{
		IReadOnlyCollection<CultureInfo> GetCultures(CultureInfo parentCulture);

		void Execute(Action action, CultureInfo cultureInfo);

		Task ExecuteAsync(Func<Task> action, CultureInfo cultureInfo);

		TResult Execute<TResult>(Func<TResult> func, CultureInfo cultureInfo);

		Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, CultureInfo cultureInfo);

		IDisposable UseCulture(CultureInfo cultureInfo);
	}
}