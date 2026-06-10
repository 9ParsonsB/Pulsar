namespace Observatory.Framework.Files.ParameterTypes;

using Converters;
using System.Text.Json.Serialization;

public class InfluenceType
{
    public ulong SystemAddress { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrendValue Trend { get; init; }

    [JsonConverter(typeof(RepInfConverter))]
    public int Influence { get; init; }
}