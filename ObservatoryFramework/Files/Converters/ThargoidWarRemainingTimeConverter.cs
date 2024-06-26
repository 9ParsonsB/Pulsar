﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Converters;

class ThargoidWarRemainingTimeConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            var dayCount = int.TryParse(value.Split(' ')[0], out var days)
                ? days
                : 0;

            return dayCount;
        }

        return reader.GetInt32();
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value + " Days");
    }
}