#region

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.IoC
{
	public sealed class UnityContainerFixed : UnityContainer
	{
		private bool _disposed;

		#region Overrides of UnityContainer

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;
			base.Dispose(disposing);
		}

		#endregion
	}
}