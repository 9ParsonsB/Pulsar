using System.Text.Json;
using System.Text.Json.Serialization;

namespace Observatory.Framework;

public class JournalInvalidDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var success = reader.TryGetDouble(out var value);
        if (success)
        return value;

        return 0;
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}