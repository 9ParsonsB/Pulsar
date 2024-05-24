using System.Text.Json;
using System.Text.Json.Serialization;

namespace Observatory.Framework;

public class JournalInvalidFloatConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var success = reader.TryGetSingle(out var value);
        if (success)
            return value;

        return 0;
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}