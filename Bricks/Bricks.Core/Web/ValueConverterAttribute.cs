#region

using System;
using System.Globalization;

#endregion

namespace Bricks.Core.Web
{
	/// <summary>
	/// Атрибут свойства для указания конвертера.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ValueConverterAttribute : Attribute
	{
		public ValueConverterAttribute(Type valueConverterType)
		{
			var valueConverterInterfaceType = typeof(IValueConverter);
			if (!valueConverterInterfaceType.IsAssignableFrom(valueConverterType))
			{
				var message = string.Format(
					CultureInfo.InvariantCulture,
					Resources.ValueConverterAttribute_Ctr_ValueConverterType_ArgumentExceptionMessage,
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