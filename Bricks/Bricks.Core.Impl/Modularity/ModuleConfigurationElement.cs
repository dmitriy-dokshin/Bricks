#region

using System;
using System.Configuration;

using Bricks.Core.Configuration;
using Bricks.Core.Modularity;

#endregion

namespace Bricks.Core.Impl.Modularity
{
	/// <summary>
	/// Настройки модуля приложения.
	/// </summary>
	internal sealed class ModuleConfigurationElement : ConfigurationElement, IModuleSettings, IKeyConfigurationElement<string>
	{
		private const string NameName = "name";
		private const string TypeName = "type";
		private const string OrderName = "order";

		#region Implementation of IKeyConfigurationElement<out string>

		/// <summary>
		/// Ключ элемента.
		/// </summary>
		public string Key
		{
			get
			{
				return Name;
			}
		}

		#endregion

		#region Implementation of IModuleSettings

		/// <summary>
		/// Полное имя типа модуля.
		/// </summary>
		[ConfigurationProperty(TypeName, IsRequired = true)]
		public string Type
		{
			get
			{
				return (string)this[TypeName];
			}
		}

		/// <summary>
		/// Название модуля.
		/// </summary>
		[ConfigurationProperty(NameName, IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)this[NameName];
			}
		}

		/// <summary>
		/// Тип модуля.
		/// </summary>
		Type IModuleSettings.Type
		{
			get
			{
				return System.Type.GetType(Type, true);
			}
		}

		/// <summary>
		/// Порядок инициализации.
		/// </summary>
		[ConfigurationProperty(OrderName, IsRequired = false, DefaultValue = int.MaxValue)]
		public int Order
		{
			get
			{
				return (int)this[OrderName];
			}
		}

		#endregion
	}
}