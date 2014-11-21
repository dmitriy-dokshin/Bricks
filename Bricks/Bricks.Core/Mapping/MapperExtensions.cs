#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Bricks.Core.Mapping
{
	public static class MapperExtensions
	{
		public static IEnumerable<TDestination> DynamicMap<TSource, TDestination>(this IMapper mapper, IEnumerable<TSource> target)
		{
			if (target == null)
			{
				return null;
			}

			return target.Select(mapper.DynamicMap<TSource, TDestination>);
		}

		public static IReadOnlyCollection<TDestination> DynamicMap<TSource, TDestination>(this IMapper mapper, IReadOnlyCollection<TSource> target)
		{
			if (target == null)
			{
				return null;
			}

			return DynamicMap<TSource, TDestination>(mapper, (IEnumerable<TSource>)target).ToArray();
		}
	}
}