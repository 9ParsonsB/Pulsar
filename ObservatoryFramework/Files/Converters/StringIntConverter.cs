using System.Text.Json;
using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Converters;

class StringIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return int.Parse(reader.GetString());
        return reader.GetInt32();
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}