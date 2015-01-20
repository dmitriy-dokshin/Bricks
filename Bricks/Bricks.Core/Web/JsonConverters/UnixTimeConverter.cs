#region

using System;

using Bricks.Core.DateTime;

using Microsoft.Practices.ServiceLocation;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#endregion

namespace Bricks.Core.Web.JsonConverters
{
	public sealed class UnixTimeConverter : DateTimeConverterBase
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public UnixTimeConverter()
		{
			_dateTimeProvider = ServiceLocator.Current.GetInstance<IDateTimeProvider>();
		}

		#region Overrides of JsonConverter

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(_dateTimeProvider.ToUnixTime((DateTimeOffset)value));
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>
		/// The object value.
		/// </returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			double? unixTime = serializer.Deserialize<double?>(reader);
			return unixTime.HasValue ? (object)_dateTimeProvider.FromUnixTime(unixTime.Value) : null;
		}

		#endregion
	}
}