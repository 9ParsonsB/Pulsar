namespace Observatory.Framework.Files.ParameterTypes;

using System.Text.Json.Serialization;

public class MicroResource
{
    public string Name { get; init; }
    public string Name_Localised { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MicroCategory Category { get; init; }

    public int Count { get; init; }
}