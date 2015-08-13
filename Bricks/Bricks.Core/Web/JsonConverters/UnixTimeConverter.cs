#region

using System;
using Bricks.Core.DateTime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#endregion

namespace Bricks.Core.Web.JsonConverters
{
	public sealed class UnixTimeConverter : DateTimeConverterBase
	{
		#region Overrides of JsonConverter

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else
			{
				if (value is System.DateTime)
				{
					writer.WriteValue(DateTimeHelper.ToUnixTime((System.DateTime) value));
				}
				else if (value is DateTimeOffset)
				{
					writer.WriteValue(DateTimeHelper.ToUnixTime((DateTimeOffset) value));
				}
				else
				{
					throw new NotSupportedException();
				}
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var unixTime = serializer.Deserialize<double?>(reader);
			object result;
			if (!unixTime.HasValue)
			{
				result = null;
			}
			else
			{
				if (objectType == typeof (System.DateTime) || objectType == typeof (System.DateTime?))
				{
					result = DateTimeHelper.FromUnixTime(unixTime.Value);
				}
				else if (objectType == typeof (DateTimeOffset) || objectType == typeof (DateTimeOffset?))
				{
					result = (DateTimeOffset) DateTimeHelper.FromUnixTime(unixTime.Value);
				}
				else
				{
					throw new NotSupportedException();
				}
			}

			return result;
		}

		#endregion
	}
}