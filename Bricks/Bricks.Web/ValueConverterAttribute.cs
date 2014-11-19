#region

using System;
using System.Globalization;

using Bricks.Web.Implementation;

#endregion

namespace Bricks.Web
{
	/// <summary>
	/// Атрибут свойства для указания конвертера.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValueConverterAttribute : Attribute
	{
		public ValueConverterAttribute(Type valueConverterType)
		{
			Type valueConverterInterfaceType = typeof (IValueConverter);
			if (!valueConverterInterfaceType.IsAssignableFrom(valueConverterType))
			{
				string message = string.Format(CultureInfo.InvariantCulture,
					ResourcesCore.ValueConverterAttribute_Ctr_ValueConverterType_ArgumentExceptionMessage,
					valueConverterInterfaceType.FullName);
				throw new ArgumentException(message);
			}

			ValueConverterType = valueConverterType;
		}

		/// <summary>
		/// Тип конвертера значений.
		/// </summary>
		public Type ValueConverterType { get; private set; }
	}
}