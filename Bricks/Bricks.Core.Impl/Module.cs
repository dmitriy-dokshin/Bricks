#region

using System;

using Bricks.Core.Modularity;
using Bricks.Core.Web;
using Bricks.Core.Web.ValueConverters;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.Impl
{
	internal sealed class Module : ModuleBase
	{
		#region Overrides of ModuleBase

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="container">The <see cref="IUnityContainer" /> that is used to store application dependencies.</param>
		/// <param name="args">The <see cref="IUnityContainer" /> that contains initialization arguments.</param>
		public override void Initialize(IUnityContainer container, IUnityContainer args)
		{
			base.Initialize(container, args);

			var webSerializationHelper = container.Resolve<IWebSerializationHelper>();
			webSerializationHelper.RegisterValueConverter<Enum, EnumToIntValueConverter>();
			webSerializationHelper.RegisterValueConverter<Guid, GuidToStringValueConverter>();
		}

		#endregion
	}
}