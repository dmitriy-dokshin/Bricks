#region

using System;
using System.Collections.Generic;
using System.Linq;

using Bricks.Core.Conversion;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Impl.Conversion
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IConverter" />.
	/// </summary>
	internal sealed class Converter : IConverter
	{
		private readonly IDictionary<Type, ICollection<object>> _convertersByDestinationType;
		private readonly IDictionary<Type, ICollection<Type>> _converterTypesByDestinationType;
		private readonly IServiceLocator _serviceLocator;

		public Converter(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
			_convertersByDestinationType = new Dictionary<Type, ICollection<object>>();
			_converterTypesByDestinationType = new Dictionary<Type, ICollection<Type>>();
		}

		#region Implementation of IConverter

		/// <summary>
		/// Пытается привести объект <paramref name="source" /> к типу <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <param name="destination">Объект целевого типа или значение по умолчанию, если не удалось сконвертировать.</param>
		/// <returns>Признак успешной конвертации.</returns>
		public bool TryConvert<TDestination>(object source, out TDestination destination)
		{
			var success = false;
			destination = default(TDestination);

			var convertible = source as IConvertible<TDestination>;
			if (convertible != null)
			{
				destination = convertible.Convert();
				success = true;
			}

			var destinationType = typeof(TDestination);
			if (!success)
			{
				ICollection<object> converters;
				if (_convertersByDestinationType.TryGetValue(destinationType, out converters))
				{
					foreach (var converter in converters.Cast<IConverter<TDestination>>())
					{
						success = converter.TryConvert(source, out destination);
						if (success)
						{
							break;
						}
					}
				}
			}

			if (!success)
			{
				ICollection<Type> converterTypes;
				if (_converterTypesByDestinationType.TryGetValue(destinationType, out converterTypes))
				{
					foreach (var converterType in converterTypes)
					{
						var converter = (IConverter<TDestination>)_serviceLocator.GetInstance(converterType);
						success = converter.TryConvert(source, out destination);
						if (success)
						{
							break;
						}
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Приводит объект <paramref name="source" /> к типу <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="source">Исходный объект.</param>
		/// <returns>Объект целевого типа.</returns>
		/// <exception cref="ConversionException">В случае если не выполнить конвертацию.</exception>
		public TDestination Convert<TDestination>(object source)
		{
			TDestination destination;
			if (!TryConvert(source, out destination))
			{
				var sourceType = source != null ? source.GetType() : null;
				throw new ConversionException(Resources.Converter_Convert_UnableToConvert, sourceType, typeof(TDestination));
			}

			return destination;
		}

		/// <summary>
		/// Регистрирует конвертер типа <typeparamref name="TConverter" /> для приведения к типу
		/// <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <typeparam name="TConverter">Тип конвертера.</typeparam>
		public void Register<TDestination, TConverter>() where TConverter : IConverter<TDestination>
		{
			var destinationType = typeof(TDestination);
			ICollection<Type> converterTypes;
			if (!_converterTypesByDestinationType.TryGetValue(destinationType, out converterTypes))
			{
				converterTypes = new HashSet<Type>();
				_converterTypesByDestinationType.Add(destinationType, converterTypes);
			}

			converterTypes.Add(typeof(TConverter));
		}

		/// <summary>
		/// Регистриует конвертер <paramref name="converter" /> для приведения к типу <typeparamref name="TDestination" />.
		/// </summary>
		/// <typeparam name="TDestination">Целевой тип.</typeparam>
		/// <param name="converter">Конвертер.</param>
		public void Register<TDestination>(IConverter<TDestination> converter)
		{
			var destinationType = typeof(TDestination);
			ICollection<object> converters;
			if (!_convertersByDestinationType.TryGetValue(destinationType, out converters))
			{
				converters = new HashSet<object>();
				_convertersByDestinationType.Add(destinationType, converters);
			}

			converters.Add(converter);
		}

		#endregion
	}
}