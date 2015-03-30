﻿#region

using System;

using Bricks.Core.Extensions;

using Newtonsoft.Json;

#endregion

namespace Bricks.Core.Web.JsonConverters
{
	public sealed class UnderscoreCaseToEnumConverter : JsonConverter
	{
		#region Overrides of JsonConverter

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (objectType.IsNullable() && reader.ValueType == typeof(bool) && !(bool)reader.Value)
			{
				return null;
			}

			Type enumType = null;
			if (objectType.IsEnum)
			{
				enumType = objectType;
			}
			else if (objectType.IsGenericType)
			{
				Type genericTypeDefinition = objectType.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(Nullable<>))
				{
					enumType = objectType.GetGenericArguments()[0];
				}
			}

			if (enumType == null)
			{
				throw new InvalidOperationException();
			}

			var valueString = serializer.Deserialize<string>(reader);
			if (valueString == null)
			{
				return null;
			}

			string value = valueString.FromUnderscoreToCamelCase();
			return Enum.Parse(enumType, value);
		}

		public override bool CanConvert(Type objectType)
		{
			bool canConvert = objectType.IsEnum;
			if (!canConvert)
			{
				if (objectType.IsGenericType)
				{
					Type genericTypeDefinition = objectType.GetGenericTypeDefinition();
					if (genericTypeDefinition == typeof(Nullable<>))
					{
						canConvert = objectType.GetGenericArguments()[0].IsEnum;
					}
				}
			}

			return canConvert;
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