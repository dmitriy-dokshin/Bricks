#region

using System;
using System.Configuration;
using System.Linq.Expressions;

using Bricks.Core.Extensions;
using Bricks.Core.Reflection;

#endregion

namespace Bricks.Core.Configuration
{
	public abstract class ConfigurationElementBase : ConfigurationElement
	{
		public TValue GetValue<TValue>(Expression<Func<TValue>> expression)
		{
			string memberName = expression.GetMemberName();
			return (TValue)this[memberName];
		}

		#region Overrides of ConfigurationElement

		protected override void PostDeserialize()
		{
			foreach (ConfigurationProperty property in Properties)
			{
				var configElement = this[property] as ConfigurationElement;
				if (configElement != null)
				{
					ElementInformation elementInformation = configElement.ElementInformation;
					if (!elementInformation.IsPresent && elementInformation.Type.IsNullable())
					{
						this[property] = null;
					}
				}
			}

			base.PostDeserialize();
		}

		#endregion
	}
}