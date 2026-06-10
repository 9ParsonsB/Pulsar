namespace Observatory.Framework.Files.ParameterTypes;

using Converters;
using Journal.Travel;
using System.Text.Json.Serialization;

public class Route
{
    public string StarSystem { get; init; }
    public ulong SystemAddress { get; init; }

    [JsonConverter(typeof(StarPosConverter))]
    public StarPos StarPos { get; init; }

    public string StarClass { get; init; }
}