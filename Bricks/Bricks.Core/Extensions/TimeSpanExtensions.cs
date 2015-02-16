#region

using System;
using System.Text;

#endregion

namespace Bricks.Core.Extensions
{
	/// <summary>
	/// Расширения для <see cref="TimeSpan" />.
	/// </summary>
	public static class TimeSpanExtensions
	{
		/// <summary>
		/// Округляет до целых секунд.
		/// </summary>
		/// <param name="timeSpan">Период времени.</param>
		/// <returns>Общее количество секунд.</returns>
		public static int RoundToSeconds(this TimeSpan timeSpan)
		{
			return Convert.ToInt32(Math.Round(timeSpan.TotalSeconds));
		}

		/// <summary>
		/// Округляет до целых минут.
		/// </summary>
		/// <param name="timeSpan">Период времени.</param>
		/// <returns>Общее количество минут.</returns>
		public static int RoundToMinutes(this TimeSpan timeSpan)
		{
			return Convert.ToInt32(Math.Round(timeSpan.TotalMinutes));
		}

		public static TimeSpan Multiply(this TimeSpan timeSpan, int multiplier)
		{
			long ticks = timeSpan.Ticks * multiplier;
			return TimeSpan.FromTicks(ticks);
		}

		public static TimeSpan Multiply(this TimeSpan timeSpan, long multiplier)
		{
			long ticks = timeSpan.Ticks * multiplier;
			return TimeSpan.FromTicks(ticks);
		}

		public static TimeSpan Multiply(this TimeSpan timeSpan, float multiplier)
		{
			float ticks = timeSpan.Ticks * multiplier;
			return TimeSpan.FromTicks(Convert.ToInt64(ticks));
		}

		public static TimeSpan Multiply(this TimeSpan timeSpan, double multiplier)
		{
			double ticks = timeSpan.Ticks * multiplier;
			return TimeSpan.FromTicks(Convert.ToInt64(ticks));
		}

		public static string ToDetailedString(this TimeSpan target)
		{
			var stringBuilder = new StringBuilder();
			if (target.Hours > 0)
			{
				stringBuilder.AppendFormat(Resources.TimeSpanPartFormat, target.Hours, Resources.Hours);
			}

			if (target.Minutes > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(Resources.TimeSpanPartsSeparator);
				}

				stringBuilder.AppendFormat(Resources.TimeSpanPartFormat, target.Minutes, Resources.Minutes);
			}

			if (target.Seconds > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(Resources.TimeSpanPartsSeparator);
				}

				stringBuilder.AppendFormat(Resources.TimeSpanPartFormat, target.Seconds, Resources.Seconds);
			}

			return stringBuilder.ToString();
		}
	}
}