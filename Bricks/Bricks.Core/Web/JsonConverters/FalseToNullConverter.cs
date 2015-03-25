#region

using System;

using Bricks.Core.Extensions;

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Web.JsonConverters
{
	public class FalseToNullConverter : JsonConverter
	{
		#region Overrides of JsonConverter

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.ValueType == typeof(bool) && !(bool)reader.Value)
			{
				return null;
			}

			object value = serializer.Deserialize(reader, objectType);
			return value;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.IsNullable();
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		#endregion
	}
}