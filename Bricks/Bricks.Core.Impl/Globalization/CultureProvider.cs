#region

using System.Globalization;

using Bricks.Core.Globalization;

#endregion

namespace Bricks.Core.Impl.Globalization
{
	internal sealed class CultureProvider : ICultureProvider
	{
		#region Implementation of ICultureProvider

		public CultureInfo CurrentCulture
		{
			get
			{
				return CultureInfo.CurrentCulture;
			}
		}

		public CultureInfo CurrentUICulture
		{
			get
			{
				return CultureInfo.CurrentUICulture;
			}
		}

		#endregion
	}
}