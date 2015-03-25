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

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(_dateTimeProvider.ToUnixTime((DateTimeOffset)value));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var unixTime = serializer.Deserialize<double?>(reader);
			return unixTime.HasValue ? (object)_dateTimeProvider.FromUnixTime(unixTime.Value) : null;
		}

		#endregion
	}
}