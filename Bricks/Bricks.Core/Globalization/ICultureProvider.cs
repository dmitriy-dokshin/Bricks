#region

using System.Globalization;

#endregion

namespace Bricks.Core.Globalization
{
	public interface ICultureProvider
	{
		CultureInfo CurrentCulture { get; }

		CultureInfo CurrentUICulture { get; }
	}
}