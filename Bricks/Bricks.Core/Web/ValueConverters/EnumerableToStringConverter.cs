#region

using System.Collections;
using System.Text;

#endregion

namespace Bricks.Core.Web.ValueConverters
{
	public sealed class EnumerableToStringConverter : ValueConverterBase<IEnumerable, string>
	{
		#region Overrides of ValueConverterBase<IEnumerable,string>

		/// <summary>
		/// Конвертирует исходное значение <paramref name="source" /> в целевое значение.
		/// </summary>
		/// <param name="source">Исходное значение.</param>
		/// <returns>Результат конвертации.</returns>
		protected override string Convert(IEnumerable source)
		{
			var sb = new StringBuilder();
			foreach (object item in source)
			{
				if (item != null)
				{
					string itemString = item.ToString();
					if (sb.Length > 0)
					{
						sb.Append(',');
					}

					sb.Append(itemString);
				}
			}

			return sb.Length > 0 ? sb.ToString() : null;
		}

		#endregion
	}
}