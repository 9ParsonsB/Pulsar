using System.Text.Json;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.Journal.Travel;

namespace Observatory.Framework.Files.Converters;

/// <summary>
/// Converting the ordered array of coordinates from the journal to a named tuple for clarity.
/// </summary>
public class StarPosConverter : JsonConverter<StarPos>
{
    public override StarPos Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var values = JsonSerializer.Deserialize<double[]>(ref reader, options)!;

        return new StarPos { X = values[0], Y = values[1], Z = values[2] };
    }

    public override void Write(Utf8JsonWriter writer, StarPos value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}