using System.Text.Json;
using System.Text.Json.Serialization;

namespace Observatory.Framework.Files.Converters;

class PipConverter : JsonConverter<(int Sys, int Eng, int Wep)>
{
    public override (int Sys, int Eng, int Wep) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var values = JsonSerializer.Deserialize<int[]>(ref reader);

        return (Sys: values[0], Eng: values[1], Wep: values[2]);
    }

    public override void Write(Utf8JsonWriter writer, (int Sys, int Eng, int Wep) value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, new {value.Sys, value.Eng, value.Wep}, options);
    }
}