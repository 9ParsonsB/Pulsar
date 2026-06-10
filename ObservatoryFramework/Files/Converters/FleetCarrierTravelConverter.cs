namespace Observatory.Framework.Files.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

internal class FleetCarrierTravelConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return float.Parse(reader.GetString().Split(' ')[0]);
        return reader.GetSingle();
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}