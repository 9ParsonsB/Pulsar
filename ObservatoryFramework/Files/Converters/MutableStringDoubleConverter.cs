using System.Text.Json;
using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Converters;

class MutableStringDoubleConverter : JsonConverter<object>
{
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return reader.GetString();
        return reader.GetDouble();
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value.GetType() == typeof(string))
            writer.WriteStringValue((string)value);
        else
            writer.WriteNumberValue((double)value);
    }
}