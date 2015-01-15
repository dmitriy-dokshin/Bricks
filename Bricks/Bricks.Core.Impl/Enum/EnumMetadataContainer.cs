#region

using System;
using System.Collections.Immutable;

using Bricks.Core.Enum;
using Bricks.Core.Sync;

using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.Impl.Enum
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IEnumMetadataContainer" />.
	/// </summary>
	internal sealed class EnumMetadataContainer : IEnumMetadataContainer
	{
		private IImmutableDictionary<Type, IEnumMetadata> _enumMetadatasByType;
		private readonly IEnumHelper _enumHelper;
		private readonly IInterlockedHelper _interlockedHelper;
		private readonly IUnityContainer _unityContainer;

		public EnumMetadataContainer(IInterlockedHelper interlockedHelper, IEnumHelper enumHelper, IUnityContainer unityContainer)
		{
			_interlockedHelper = interlockedHelper;
			_enumHelper = enumHelper;
			_unityContainer = unityContainer;
			_enumMetadatasByType = ImmutableDictionary<Type, IEnumMetadata>.Empty;
		}

		private IEnumMetadata CreateEnumMetadata(Type enumType)
		{
			var dependencyOverride = new DependencyOverride(typeof(Type), new InjectionParameter(enumType));
			var enumMetadata =
				_enumHelper.IsFlags(enumType)
					? _unityContainer.Resolve<IFlagsMetadata>(dependencyOverride)
					: _unityContainer.Resolve<IEnumMetadata>(dependencyOverride);
			return enumMetadata;
		}

		#region Implementation of IEnumMetadataContainer

		/// <summary>
		/// Получает метаданные перечисления типа <paramref name="enumType" />.
		/// </summary>
		/// <param name="enumType">Тип перечисления.</param>
		/// <returns>Метаданные перечисления.</returns>
		public IEnumMetadata GetEnumMetadata(Type enumType)
		{
			var result = _interlockedHelper.CompareExchange(ref _enumMetadatasByType, x =>
				{
					var enumMetadatasByType = _enumMetadatasByType;
					IEnumMetadata enumMetadata;
					if (!_enumMetadatasByType.TryGetValue(enumType, out enumMetadata))
					{
						enumMetadata = CreateEnumMetadata(enumType);
						enumMetadatasByType = enumMetadatasByType.Add(enumType, enumMetadata);
					}

					return _interlockedHelper.CreateChangeResult(enumMetadatasByType, enumMetadata);
				});
			return result;
		}

		public IEnumMetadata<TEnum> GetEnumMetadata<TEnum>() where TEnum : struct
		{
			return new EnumMetadataTAdapter<TEnum>(GetEnumMetadata(typeof(TEnum)));
		}

		#endregion
	}
}