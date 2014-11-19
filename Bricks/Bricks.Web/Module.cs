#region

using System;

using Bricks.Core.Modularity.Implementation;
using Bricks.Web.ValueConverters;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Web
{
	internal sealed class Module : ModuleBase
	{
		#region Overrides of ModuleBase

		/// <summary>
		/// Initializes the module.
		/// </summary>
		/// <param name="container">
		/// The <see cref="Microsoft.Practices.Unity.IUnityContainer" /> that is used to store application
		/// dependencies.
		/// </param>
		/// <param name="args">The <see cref="Microsoft.Practices.Unity.IUnityContainer" /> that contains initialization arguments.</param>
		public override void Initialize(IUnityContainer container, IUnityContainer args)
		{
			base.Initialize(container, args);

			var webSerializationHelper = container.Resolve<IWebSerializationHelper>();
			webSerializationHelper.RegisterValueConverter<Enum, EnumToLowerStringValueConverter>();
			webSerializationHelper.RegisterValueConverter<Guid, GuidToStringValueConverter>();
		}

		#endregion
	}
}