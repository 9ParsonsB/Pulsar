using System.Text.Json.Serialization;
using Observatory.Framework.Files.Converters;
using Observatory.Framework.Files.Journal.Travel;

namespace Observatory.Framework.Files.ParameterTypes;

public class Route
{
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }
    [JsonConverter(typeof(StarPosConverter))]
    public StarPos StarPos { get; init; }
    public string StarClass { get; init; }
}