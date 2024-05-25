using System.Text.Json;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Converters;

class NumberOrStringConverter : JsonConverter<NumberOrString>
{
    public override NumberOrString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String ? new NumberOrString(reader.GetString()) : new NumberOrString(reader.GetDouble());
    }

    public override void Write(Utf8JsonWriter writer, NumberOrString value, JsonSerializerOptions options)
    {
        if (value.IsString)
            writer.WriteStringValue(value.StringValue!);
        else
            writer.WriteNumberValue(value.DoubleValue!.Value);
    }
}