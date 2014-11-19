#region

using System;

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
	}
}